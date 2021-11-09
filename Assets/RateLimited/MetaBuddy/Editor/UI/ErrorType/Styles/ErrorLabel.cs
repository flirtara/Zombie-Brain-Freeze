using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorType.Styles
{
    public class ErrorLabel : IStyleFactory
    {
        public GUIStyle Create()
        {
            var matchStyle = EditorStyles.miniLabel;

            var style = new GUIStyle(EditorStyles.miniBoldLabel)
            {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = StyleConstants.ErrorColor },
                margin =
                {
                    top = matchStyle.margin.top,
                    bottom = matchStyle.margin.bottom,
                    left = matchStyle.margin.left,
                    right = 0
                },
                padding =
                {
                    top = matchStyle.padding.top,
                    bottom = matchStyle.padding.bottom,
                    left = matchStyle.padding.left,
                    right = 0
                },
                border =
                {
                    top = matchStyle.border.top,
                    bottom = matchStyle.border.bottom,
                    left = matchStyle.border.left,
                    right = 0
                }
            };          

            return style;
        }
    }
}
