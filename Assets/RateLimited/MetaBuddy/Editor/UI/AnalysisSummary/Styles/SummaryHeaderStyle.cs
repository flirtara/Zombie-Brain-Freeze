using UnityEditor;
using UnityEngine;
using MetaBuddy.UI.Styles;
using System;

namespace MetaBuddy.UI.AnalysisSummary.Styles
{
    public class SummaryHeaderStyle : IStyleFactory
    {
        private readonly Color _textColor;

        public SummaryHeaderStyle(Color textColor)
        {
            _textColor = textColor;
        }

        public GUIStyle Create()
        {
            var style = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold,
                fontSize = Convert.ToInt32(EditorStyles.largeLabel.fontSize * 1.5f),
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = _textColor }
            };

            return style;
        }
    }
}
