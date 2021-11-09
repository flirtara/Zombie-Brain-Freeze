using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.AnalysisSummary.Styles
{
    public class StatLabelStyle : IStyleFactory
    {
        public GUIStyle Create()
        {
            var textColor = EditorStyles.miniLabel.normal.textColor;
            textColor.a = StyleConstants.TwoThirdsOpacity;

            var style = new GUIStyle(EditorStyles.miniLabel)
            {
                fontStyle = FontStyle.Bold,
                normal = { textColor = textColor },
                hover = { textColor = textColor },
                alignment = TextAnchor.MiddleCenter                
            };         

            return style;
        }
    }
}
