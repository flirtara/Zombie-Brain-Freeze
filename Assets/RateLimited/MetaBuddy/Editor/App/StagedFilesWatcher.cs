using System;
using System.IO;
using MetaBuddyLib.Log;

namespace MetaBuddy.App
{
    public class StagedFilesWatcher
    {
        private FileSystemWatcher _watcher;
        private IMetaBuddyLogger _logger;
        public DateTime LastStageTime { get; private set; } = DateTime.Now;

        private void UpdateLastIndexModification(FileSystemEventArgs e)
        {
            try
            {
                LastStageTime = File.GetLastWriteTime(e.FullPath);
                _logger.LogDebug($"Staged files updated at {LastStageTime}");
            }
            catch
            {
            }
        }

        private FileSystemWatcher CreateWatcher(string repositoryPath)
        {           
            var watcher = new FileSystemWatcher
            (
                Path.Combine(repositoryPath, ".git"),
                "index"
            );

            watcher.NotifyFilter = NotifyFilters.LastWrite
                | NotifyFilters.CreationTime;

            watcher.Changed += (s, e) => UpdateLastIndexModification(e);
            watcher.Created += (s, e) => UpdateLastIndexModification(e);

            return watcher;
        }

        public void TryStartWatcher(string repositoryPath)
        {
            if (_watcher == null)
            {
                try 
                {
                    _logger.LogDebug("Starting staged files watcher.");
                    _watcher = CreateWatcher(repositoryPath);
                    _watcher.EnableRaisingEvents = true;
                    _logger.LogDebug("Watcher started sucessfully.");
                }
                catch
                {
                }
            }
        }

        public StagedFilesWatcher(string repositoryPath, IMetaBuddyLogger logger)
        {
            _logger = logger;
            TryStartWatcher(repositoryPath);
        }
    }
}
