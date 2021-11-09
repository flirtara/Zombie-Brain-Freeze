using MetaBuddyLib.Config;
using MetaBuddy.Settings;
using UnityEditor;
using UnityEngine;
using MetaBuddy.App;
using MetaBuddy.UI.Styles;
using MetaBuddy.UI.Settings.Styles;
using MetaBuddy.UI.Footer.Styles;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace MetaBuddy.UI.Settings
{
    public class MetaBuddySettingsProvider : SettingsProvider
    {
        public static readonly string SettingsUIPath = $"Project/{Product.Name}";

        private ConfigModel _config;
        private SerializedObject _serializedSettings;

        private readonly StyleCache _settingsContainerStyle = new StyleCache(new SettingsContainer());
        private readonly StyleCache _footerContainerStyle = new StyleCache(new FooterContainer());

        private SerializedProperty _suppressBannerProperty;
        private SerializedProperty _ignoreFilesInDotGitIgnoreProperty;
        private SerializedProperty _verboseLoggingProperty;
        private SerializedProperty _runOnEditorStartProperty;
        private SerializedProperty _logIgnoredFilesProperty;
        private SerializedProperty _autoAnalyseOnStageChangeProperty;
        private SerializedProperty _noCorrectionsProperty;

        private class Labels
        {
            public static GUIContent SuppressBanner = new GUIContent("Hide banner in Console and log files");
            public static GUIContent IgnoreFilesFromDotGitIgnore = new GUIContent("Ignore files from .gitignore");
            public static GUIContent RunOnEditorStart = new GUIContent("Run analysis when Unity Editor starts");
            public static GUIContent VerboseLogging = new GUIContent("Verbose logging");
            public static GUIContent LogIgnoredFiles = new GUIContent("Log ignored files to the console");
            public static GUIContent AutoAnalyseOnStageChange = new GUIContent("Auto-analyse when files are (un)staged");
            public static GUIContent NoCorrections = new GUIContent("Report errors that fix pre-existing problems");
            public static GUIContent ProductTitle = new GUIContent(Product.Title, "Visit the MetaBuddy home page.");
        }

        private MetaBuddySettingsProvider
        (
            string uiPath,
            ConfigModel config,
            SettingsScope scope
        ) : base(uiPath, scope)
        {
            _config = config;
        }

        private void InitSettings()
        {
            _serializedSettings = CreateSettingsFromConfig();

            _suppressBannerProperty = _serializedSettings.FindProperty(MetaBuddySettings.SuppressBannerPropertyName);
            _ignoreFilesInDotGitIgnoreProperty = _serializedSettings.FindProperty(MetaBuddySettings.IgnoreFilesInDotGitIgnorePropertyName);
            _verboseLoggingProperty = _serializedSettings.FindProperty(MetaBuddySettings.VerboseLoggingPropertyName);
            _runOnEditorStartProperty = _serializedSettings.FindProperty(MetaBuddySettings.RunOnEditorStartPropertyName);
            _logIgnoredFilesProperty = _serializedSettings.FindProperty(MetaBuddySettings.LogIgnoredFilesPropertyName);
            _autoAnalyseOnStageChangeProperty = _serializedSettings.FindProperty(MetaBuddySettings.AutoAnalyseOnStageChangePropertyName);
            _noCorrectionsProperty = _serializedSettings.FindProperty(MetaBuddySettings.NoCorrectionsName);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            InitSettings();
        }

        public override void OnGUI(string searchContext)
        {
            if (_serializedSettings.targetObject == null)
            {
                InitSettings();
            }

            EditorGUILayout.BeginVertical(_settingsContainerStyle.Get);

            var saveLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 250;

            EditorGUILayout.PropertyField
            (
                _suppressBannerProperty,
                Labels.SuppressBanner
            );

            EditorGUILayout.PropertyField
            (
                _ignoreFilesInDotGitIgnoreProperty,
                Labels.IgnoreFilesFromDotGitIgnore
            );

            EditorGUILayout.PropertyField
            (
                _verboseLoggingProperty,
                Labels.VerboseLogging
            );

            EditorGUILayout.PropertyField
            (
                _runOnEditorStartProperty,
                Labels.RunOnEditorStart
            );

            EditorGUILayout.PropertyField
            (
                _logIgnoredFilesProperty,
                Labels.LogIgnoredFiles
            );

            EditorGUILayout.PropertyField
            (
                _autoAnalyseOnStageChangeProperty,
                Labels.AutoAnalyseOnStageChange
            );

            EditorGUILayout.PropertyField
            (
                _noCorrectionsProperty,
                Labels.NoCorrections
            );

            EditorGUIUtility.labelWidth = saveLabelWidth;

            EditorGUILayout.EndVertical();

            if(_serializedSettings.hasModifiedProperties)
            {
                _serializedSettings.ApplyModifiedPropertiesWithoutUndo();
                SaveSettingsToConfig();                
            }       
        }

        public override void OnTitleBarGUI()
        {
            if (GUIUtil.HelpIcon())
            {
                Product.OpenDocumentationPage();
            }
        }

        public override void OnFooterBarGUI()
        {
            EditorGUILayout.BeginVertical(_footerContainerStyle.Get);
                Footer.FooterGUI.Generate(false);
            EditorGUILayout.EndVertical();
        }

        private SerializedObject CreateSettingsFromConfig()
        {
            var settings = MetaBuddySettings.CreateFromConfig(_config);           
            return new SerializedObject(settings);
        }

        private void SaveSettingsToConfig()
        {
            var settings = _serializedSettings.targetObject as MetaBuddySettings;

            ConfigSerialization.Save(settings.ToConfig(), ConfigModel.DefaultConfigFilename);
            settings.UpdateConfig(_config);
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new MetaBuddySettingsProvider
            (
                SettingsUIPath,
                ServiceLocator.Registry.Config,
                SettingsScope.Project
            )
            {
                keywords = GetSearchKeywordsFromGUIContentProperties<Labels>()
            };

            return provider;
        }
    }
}