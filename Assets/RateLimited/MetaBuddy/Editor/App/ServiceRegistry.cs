using MetaBuddy.Analysis;
using MetaBuddy.Util;
using MetaBuddyLib.Config;
using MetaBuddyLib.Log;

namespace MetaBuddy.App
{
    public class ServiceRegistry
    {
        public ConfigModel Config { get; set; }

        private AnalysisModel AnalysisModel;

        public IMetaBuddyLogger Logger { get; private set; }

        public AnalysisController AnalysisController { get; private set; }

        public IReadOnlyAnalysisModel ReadOnlyAnalysisModel
        {
            get
            {
                return AnalysisModel;
            }
        }

        private static AnalysisController CreateController
        (
            ConfigModel config,
            AnalysisModel model,
            UnityLogger logger
        )
        {
            var exceptionLogger = new ExceptionLogger(logger);
            var checkerFactory = new CheckerFactory(logger);
            var runner = new AnalysisRunner
            (
                logger,
                checkerFactory,
                exceptionLogger
            );

            var watcher = new StagedFilesWatcher
            (
                config.RepositoryPath,
                logger
            );

            return new AnalysisController
            (
                model,
                runner,
                watcher,
                logger,
                config
            );
        }

        public ServiceRegistry()
        {
            var logger = new UnityLogger();

            var model = new AnalysisModel();

            var configFetcher = new ConfigFetcher(logger);
            var config = configFetcher.FetchConfig();

            var controller = CreateController(config, model, logger);

            Logger = logger;
            AnalysisModel = model;
            Config = config;
            AnalysisController = controller;

            if (Config.NoBanner == false)
            {
                Logger.LogNone(Product.Banner);
            }
        }
    }
}
