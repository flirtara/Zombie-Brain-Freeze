using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.AnalysisSummary.Styles
{
    public class StatValueStyle : IStyleFactory
    {
        public GUIStyle Create()
        {
            var textColor = EditorStyles.largeLabel.normal.textColor;
            textColor.a = StyleConstants.TwoThirdsOpacity;

            var style = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold,
                fontSize = EditorStyles.largeLabel.fontSize * 2,
                normal = { textColor = textColor },
                hover = { textColor = textColor },
                alignment = TextAnchor.MiddleCenter,
                padding =
                {
                    top = 0,
                    bottom = 0,
                    left = 0,
                    right = 0,
                },
                margin =
                {
                    top = 0,
                    bottom = 0,
                    left = 0,
                    right = 0,
                },
                border =
                {
                    top = 0,
                    bottom = 0,
                    left = 0,
                    right = 0,
                }
            };         

            return style;
        }
    }
}
