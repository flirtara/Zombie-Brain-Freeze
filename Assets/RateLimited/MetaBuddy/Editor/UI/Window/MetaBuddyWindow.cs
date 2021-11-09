using UnityEditor;
using UnityEngine;
using MetaBuddyLib.Analysis;
using MetaBuddy.App;
using MetaBuddyLib.Log;
using MetaBuddy.UI.Styles;
using MetaBuddy.UI.ErrorList;
using MetaBuddy.UI.Window.Styles;
using MetaBuddy.UI.Settings;
using MetaBuddy.UI.AnalysisSummary;
using MetaBuddy.Util;
using System.IO;
using MetaBuddy.UI.HelpBox;
using MetaBuddyLib.Config;

namespace MetaBuddy.UI.Window
{
    public class MetaBuddyWindow : EditorWindow
    {
        private ConfigModel _config;
        private IReadOnlyAnalysisModel _model;
        private AnalysisController _controller;

        private ErrorListGUI _errorsView;
        private readonly StyleCache _analyseButtonStyle = new StyleCache(new AnalyseButton());
        private readonly AnalysisSummaryGUI _summaryView = new AnalysisSummaryGUI();
        private readonly HelpBoxGUI _helpBox = new HelpBoxGUI();

        private IMetaBuddyLogger _logger;
        private PathResolver _imagePathResolver;

        private CachedTexture _analyseIcon;
        private CachedTexture _buddyIcon;

        private PathResolver CreateImagePathResolver()
        {
            var skinPath = (EditorGUIUtility.isProSkin)
                 ? "pro"
                 : "personal";

            var imagePath = Path.Combine("images", skinPath);

            return PathResolver.Create(Product.PackageName, this, imagePath, 1);
        }

        private PathResolver ImagePathResolver
        {
            get
            {
                _imagePathResolver = _imagePathResolver ?? CreateImagePathResolver();
                return _imagePathResolver;
            }
        }

        private IMetaBuddyLogger Logger
        {
            get
            {
                _logger = _logger ?? ServiceLocator.Registry.Logger;
                return _logger;
            }            
        }

        public void OnEnable()
        {
            var registry = ServiceLocator.Registry;
            _config = registry.Config;
            _model = registry.ReadOnlyAnalysisModel;
            _controller = registry.AnalysisController;

            _errorsView = new ErrorListGUI();

            Logger.LogDebug($"Loading UI images from {ImagePathResolver.ProjectRelativeBasePath}");

            _analyseIcon = new CachedTexture
            (
                 ImagePathResolver.ResolvePath("noun_play_5206-32x32.png")
            );

            _buddyIcon = new CachedTexture
            (
                 ImagePathResolver.ResolvePath("noun_Buddy_3361021-32x32.png")
            );

            _summaryView.OnEnable(ImagePathResolver);

            titleContent = new GUIContent(Product.Name, _buddyIcon.Get);

        }

        private void PresentResults(AnalysisResults results, string projectPath)
        {
            EditorGUILayout.Space();

            if (results.Assets.AnalysedFileCount > 0)
            {
                _summaryView.Generate(results);

                EditorGUILayout.Space();

                _errorsView.Generate
                (
                    results.Errors,
                    results.Assets.BasePath,
                    projectPath
                );
            }
            else
            {
                _helpBox.Generate
                (
                    "\n" +
                    $"No staged changes for {Product.Name} to check.\n\n" +
                    "Try staging some files for commit with 'git add' before checking again.\n",
                    MessageType.Info
                );
            }
        }

        private static void IconBar()
        {
            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUIUtil.HelpIcon())
                {
                    Product.OpenDocumentationPage();
                }

                if (GUIUtil.GearIcon())
                {
                    SettingsService.OpenProjectSettings(MetaBuddySettingsProvider.SettingsUIPath);
                }
              
            EditorGUILayout.EndHorizontal();
        }

        private bool LastCriticalErrorGUI()
        {
            if(_model.LastError != null)
            { 
                _helpBox.Generate(_model.LastError.Message, MessageType.Error);
                return true;
            }

            return false;
        }

        private void BodyGUI()
        {
            if (_model.AnalysisInProgress)
            {
                var checkMsg = "Checking...";
                EditorUtility.DisplayProgressBar(Product.Name, checkMsg, _model.AnalysisProgress);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(checkMsg);
            }
            else
            {
                EditorUtility.ClearProgressBar();
                if (!LastCriticalErrorGUI())
                {
                    if (_model.HasResults)
                    {
                        PresentResults(_model.Results, _config.ProjectPath);
                    }
                }
            }           
        }

        public void Update()
        {
            if(_controller != null && _controller.Update())
            {
                Repaint();
            }          
        }

        public void OnGUI()
        {
            wantsMouseMove = _model.HasErrors;

            EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                IconBar();

                EditorGUILayout.Space();
            
                var content = new GUIContent("Check Staged Changes", _analyseIcon.Get);

                if (GUILayout.Button(content, _analyseButtonStyle.Get))
                {
                    _controller.StartAnalysisAsync();
                }

                BodyGUI();
          
                GUILayout.FlexibleSpace();

                Footer.FooterGUI.Generate(true);
            
            EditorGUILayout.EndVertical();
        }

        [MenuItem("Window/MetaBuddy")]
        public static void ShowWindow()
        { 
            GetWindow(typeof(MetaBuddyWindow));
        }
    }
}

