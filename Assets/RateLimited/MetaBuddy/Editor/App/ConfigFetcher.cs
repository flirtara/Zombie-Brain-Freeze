using MetaBuddy.Settings;
using MetaBuddyLib.Config;
using MetaBuddyLib.Log;

namespace MetaBuddy.App
{
    public class ConfigFetcher
    {
        private readonly IMetaBuddyLogger _logger;

        public ConfigFetcher(IMetaBuddyLogger logger)
        {
            _logger = logger;
        }

        public ConfigModel FetchConfig()
        {
            var commandLineArgs = ArgumentParser.ParseCommandLine();
            _logger.SetDebug(commandLineArgs.Verbose);

            var baseArgs = DefaultConfigFactory.Create();

            var config = ConfigResolver.ResolveConfig
            (
                baseArgs,
                commandLineArgs,
                _logger
            );
            _logger.SetDebug(config.Verbose);

            return config;
        }
    }
}
