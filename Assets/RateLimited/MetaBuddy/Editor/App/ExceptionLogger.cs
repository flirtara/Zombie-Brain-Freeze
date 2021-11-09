using System;
using LibGit2Sharp;
using MetaBuddyLib.Log;

namespace MetaBuddy.App
{
    public class ExceptionLogger
    {
        private readonly IMetaBuddyLogger _logger;

        public ExceptionLogger(IMetaBuddyLogger logger)
        {
            _logger = logger;
        }

        private void LogUnhandled(Exception e)
        {
            _logger.LogCritical
            (
                $"Sorry! MetaBuddy encountered an unhandled exception.\n" +
                $"\n" +
                e.Message +
                $"\n" +
                e.GetType() +
                $"\n\n" +
                $"Send us the details using the links at the bottom of the MetaBuddy window and we'll take a look at getting it fixed for you."
            );
        }

        private bool TryLogRepositoryNotFound(Exception e)
        {
            if (e is RepositoryNotFoundException repoNotFound)
            {
                _logger.LogCritical
                (
                    $"MetaBuddy can't find the git repository for this project.\n" +
                    $"\n" +
                    $"This is often because git hasn't been initialised for the project." +
                    "You can do this by running 'git init' in project's root directory.\n" +
                    $"\n" +
                    $"If you're still stuck, contact us via the links at the bottom of the MetaBuddy window and we'll see what we can do to help out."
                );

                return true;
            }

            return false;
        }

        public void LogException(Exception e)
        {
            var handled = TryLogRepositoryNotFound(e);

            if(!handled)
            {
                LogUnhandled(e);
            }
        }
    }
}

