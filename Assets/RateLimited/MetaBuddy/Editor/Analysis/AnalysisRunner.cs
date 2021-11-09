using System;
using System.Threading.Tasks;
using MetaBuddy.App;
using MetaBuddyLib.Analysis;
using MetaBuddyLib.Checking;
using MetaBuddyLib.Config;
using MetaBuddyLib.Log;

namespace MetaBuddy.Analysis
{
   public class AnalysisRunner
   {
        private readonly IMetaBuddyLogger _logger;
        private readonly CheckerFactory _checkerFactory;
        private readonly ExceptionLogger _exceptionLogger;

        public AnalysisRunner
        (
            IMetaBuddyLogger logger,
            CheckerFactory checkerFactory,
            ExceptionLogger exceptionLogger
        )
        {
            _logger = logger;
            _checkerFactory = checkerFactory;
            _exceptionLogger = exceptionLogger;
        }
      
        private Task<AnalysisResults> BeginAnalysis(ConfigModel config)
        {
            var task = Task<AnalysisResults>.Factory.StartNew
            (
                () => CheckExecutor.Execute(config, _checkerFactory, _logger)
            );

            return task;
        }

        public Task<AnalysisResults> AnalyseAsync(ConfigModel config)
        {
            try
            {
                return BeginAnalysis(config);
            }
            catch (Exception e)
            {
                _exceptionLogger.LogException(e);
            }

            return null;
        }

        public bool WaitForResults
        (
            Task<AnalysisResults> task,
            out AnalysisResults results
        )
        {
            try
            {
                task.Wait();
                results = task.Result;
                return true;
            }
            catch (AggregateException ae)
            {
                var innerException = ae.InnerException;
                _exceptionLogger.LogException(innerException);
            }

            results = null;
            return false;
        }
    }
}
