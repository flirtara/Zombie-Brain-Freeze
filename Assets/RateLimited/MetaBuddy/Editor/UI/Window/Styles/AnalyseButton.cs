using System;
using MetaBuddy.UI.Styles;
using UnityEngine;

namespace MetaBuddy.UI.Window.Styles
{
    public class AnalyseButton : IStyleFactory
    {
        public GUIStyle Create()
        {
            int padding = StyleConstants.StandardPadding;

            var textColor = GUI.skin.button.normal.textColor;
            textColor.a = StyleConstants.TwoThirdsOpacity;

            var style = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = Convert.ToInt32(GUI.skin.button.fontSize * 1.5f),
                normal = { textColor = textColor },
                hover = { textColor = textColor },
                padding =
                {
                    top = padding,
                    bottom = padding,
                    left = padding,
                    right = padding,
                }
            };         

            return style;
        }
    }
}
