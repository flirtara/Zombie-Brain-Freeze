using MetaBuddyLib.Config;
using UnityEngine;

namespace MetaBuddy.Settings
{
    public class MetaBuddySettings : ScriptableObject
    {        
#pragma warning disable 0414
        [SerializeField]
        public bool suppressBanner;

        [SerializeField]
        public bool ignoreFilesInDotGitIgnore;

        [SerializeField]
        public bool verboseLogging;

        [SerializeField]
        public bool analyseOnEditorStartup;

        [SerializeField]
        public bool logIgnoredFiles;

        [SerializeField]
        public bool autoAnalyseOnStageChange;

        [SerializeField]
        public bool noCorrections;

#pragma warning restore 0414

        public const string SuppressBannerPropertyName = nameof(suppressBanner);
        public const string IgnoreFilesInDotGitIgnorePropertyName = nameof(ignoreFilesInDotGitIgnore);
        public const string VerboseLoggingPropertyName = nameof(verboseLogging);
        public const string RunOnEditorStartPropertyName = nameof(analyseOnEditorStartup);
        public const string LogIgnoredFilesPropertyName = nameof(logIgnoredFiles);
        public const string AutoAnalyseOnStageChangePropertyName = nameof(autoAnalyseOnStageChange);
        public const string NoCorrectionsName = nameof(noCorrections);

        internal static MetaBuddySettings CreateFromConfig(ConfigModel config)
        {
            var settings = CreateInstance<MetaBuddySettings>();

            settings.suppressBanner = config.NoBanner;
            settings.ignoreFilesInDotGitIgnore = !config.NoDotGitIgnore;
            settings.verboseLogging = config.Verbose;
            settings.analyseOnEditorStartup = config.AnalyseOnEditorStartup;
            settings.logIgnoredFiles = config.ListIgnoredFiles;
            settings.autoAnalyseOnStageChange = config.AutoAnalyseOnStageChange;
            settings.noCorrections = config.NoCorrections;

            return settings;
        }

        internal void UpdateConfig(ConfigModel config)
        {
            config.NoBanner = suppressBanner;
            config.NoDotGitIgnore = !ignoreFilesInDotGitIgnore;
            config.Verbose = verboseLogging;
            config.AnalyseOnEditorStartup = analyseOnEditorStartup;
            config.ListIgnoredFiles = logIgnoredFiles;
            config.AutoAnalyseOnStageChange = autoAnalyseOnStageChange;
            config.NoCorrections = noCorrections;
        }

        internal ConfigModel ToConfig()
        {
            var config = new ConfigModel()
            {
                CheckCommand = CheckCommand.Changes
            };
            UpdateConfig(config);
            return config;
        }
    }
}

