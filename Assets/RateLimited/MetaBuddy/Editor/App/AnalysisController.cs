using System;
using MetaBuddy.Analysis;
using MetaBuddy.Util;
using MetaBuddyLib.Analysis;
using MetaBuddyLib.Config;
using MetaBuddyLib.Log;

namespace MetaBuddy.App
{
    public class AnalysisController
    {
        private readonly TimeSpan AutoCheckingSettleTime = new TimeSpan(0, 0, 0, 0, 300);
        private readonly AnalysisModel _model;
        private readonly AnalysisRunner _runner;
        private readonly StagedFilesWatcher _watcher;
        private readonly ILogHistory _logHistory;
        private readonly ConfigModel _config;

        private DateTime _lastAnalysisStart = DateTime.MinValue;
        private const float MaxCompletionTime = 30.0f;

        public AnalysisController
        (
            AnalysisModel model,
            AnalysisRunner runner,
            StagedFilesWatcher watcher,
            ILogHistory logHistory,
            ConfigModel config
        )
        {
            _model = model;
            _runner = runner;
            _watcher = watcher;
            _logHistory = logHistory;
            _config = config;
        }

        public void StartAnalysisAsync()
        {
            _model.Results = null;

            _lastAnalysisStart = DateTime.Now;
            _model.LastError = null;
            _model.AnalysisTask = _runner.AnalyseAsync(_config);
        }

        private LogEntry GetLastError()
        {
            var lastLog = _logHistory.LastLogEntry;

            if ((lastLog != null) && ((lastLog.Flags & LogLevelFlags.Critical) != LogLevelFlags.None))
            {
                return lastLog;
            }

            return null;
        }

        private bool CompleteAnalysis()
        {
            var task = _model.AnalysisTask;
            _model.AnalysisTask = null;

            AnalysisResults results;
            var success = _runner.WaitForResults(task, out results);
            _model.Results = results;

            if(success)
            {
                _watcher.TryStartWatcher(_config.RepositoryPath);
            }

            _model.LastError = GetLastError();

            return success;
        }

        private void UpdateAnalysisProgress()
        {
            if(_model.AnalysisTask != null)
            {
                var elapsed = (float) (DateTime.Now - _lastAnalysisStart).TotalSeconds;
                _model.AnalysisProgress = Math.Min(elapsed / MaxCompletionTime, 1.0f);

                if (_model.AnalysisTask.IsCompleted)
                {
                    CompleteAnalysis();
                }
            }
        }

        public AnalysisResults RunAnalysisSync(AnalysisContext context)
        {
            if (context != AnalysisContext.EditorStart || _config.AnalyseOnEditorStartup)
            {

                StartAnalysisAsync();
                CompleteAnalysis();
            }

            return _model.Results;
        }

        private bool ShouldStartAutoCheck
        {
            get
            {
                var timeSinceLastStageChange = DateTime.Now - _watcher.LastStageTime;

                return
                (
                    _config.AutoAnalyseOnStageChange 
                    && (_watcher.LastStageTime > _lastAnalysisStart)
                    && (timeSinceLastStageChange > AutoCheckingSettleTime)
                );
            } 
        }  

        public bool Update()
        {
            if (_model.AnalysisInProgress)
            {
                UpdateAnalysisProgress();
                return true;
            }
            else if (ShouldStartAutoCheck)
            {
                StartAnalysisAsync();
                return true;
            }

            return false;
        }
    }
}
