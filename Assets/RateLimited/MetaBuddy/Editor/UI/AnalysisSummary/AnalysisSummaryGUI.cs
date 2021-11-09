using UnityEditor;
using UnityEngine;
using MetaBuddyLib.Analysis;
using MetaBuddy.UI.Styles;
using MetaBuddy.UI.AnalysisSummary.Styles;
using MetaBuddy.Util;

namespace MetaBuddy.UI.AnalysisSummary
{
    public class AnalysisSummaryGUI 
    {
        private readonly StyleCache _containerStyle = new StyleCache(new SummaryContainerStyle());

        private readonly StyleCache _statValueStyle = new StyleCache(new StatValueStyle());
        private readonly StyleCache _statLabelStyle = new StyleCache(new StatLabelStyle());

        private CachedTexture _okIcon;
        private CachedTexture _errorIcon;

        private StyleCache _okSummaryHeaderStyle;
        private StyleCache _errorSummaryHeaderStyle;

        public void OnEnable(PathResolver pathResolver)
        {
            _okIcon = new CachedTexture
            (
                pathResolver.ResolvePath("noun_Ok_108699-32x32.png")
            );

            _errorIcon = new CachedTexture
            (
                pathResolver.ResolvePath("noun_Error_2190948-32x32.png")
            );

            _okSummaryHeaderStyle = new StyleCache(new SummaryHeaderStyle(StyleConstants.OKColor));
            _errorSummaryHeaderStyle = new StyleCache(new SummaryHeaderStyle(StyleConstants.ErrorColor));
        }

        public void Reset() 
        {
            _okSummaryHeaderStyle.Reset();
            _errorSummaryHeaderStyle.Reset();
        }
       
        private void StatBox(string name, int count)
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label(count.ToString(), _statValueStyle.Get);
            GUILayout.Label(name, _statLabelStyle.Get);

            EditorGUILayout.EndHorizontal();
        }

        private void Header(int errorCount)
        {
            var icon = (errorCount == 0)
              ? _okIcon
              : _errorIcon;

            var headerStyle = (errorCount == 0)
                ? _okSummaryHeaderStyle
                : _errorSummaryHeaderStyle;

            var reportTitle = new GUIContent
            (
                //(errorCount == 0) ? "٩(^ᴗ^)۶" : "(┛ಠ_ಠ)┛彡┻━┻",
                (errorCount == 0) ? "No Meta Errors" : $"Meta Errors Detected",
                icon.Get
            );

            GUILayout.Label(reportTitle, headerStyle.Get);
        }

        private void StatsGrid(AnalysisResults results)
        {
            var changes = results.Assets;
            var errors = results.Errors;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            StatBox("Assets", changes.ContentFileCount);
            GUILayout.FlexibleSpace();

            StatBox("Meta Files", changes.MetaFileCount);
            GUILayout.FlexibleSpace();

            StatBox("Ignored", changes.IgnoredFileCount);
            GUILayout.FlexibleSpace();

            StatBox("Errors", errors.ErrorCount);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        public void Generate(AnalysisResults results)
        {
            EditorGUILayout.BeginVertical(_containerStyle.Get);

            Header(results.Errors.ErrorCount);

            EditorGUILayout.Space();

            StatsGrid(results);

            EditorGUILayout.EndVertical();
        }             
    }
}

