using System;
using MetaBuddyLib.Checking;
using MetaBuddyLib.Config;
using MetaBuddyLib.Log;

namespace MetaBuddy.Analysis
{
    public class CheckerFactory : ICheckerFactory
    {
        private readonly IMetaBuddyLogger _logger;

        public CheckerFactory(IMetaBuddyLogger logger)
        {
            _logger = logger;
        }

        public IChecker Create(CheckCommand checkCommand)
        {
            switch (checkCommand)
            {
                case CheckCommand.All:
                    {
                        return new ProjectChecker(_logger);
                    }

                case CheckCommand.Changes:
                    {
                        return new ChangesChecker(_logger);
                    }

                default:
                    {
                        throw new NotImplementedException("Unhandled check command");
                    }
            }

            throw new ArgumentException($"Couldn't create checker for check command '{checkCommand}'.");
        } 
    }
}
