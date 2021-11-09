using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorRow.Styles
{
    public class ErrorPrefix : IStyleFactory
    {
        public GUIStyle Create()
        {
            var opaqueColor = EditorStyles.miniLabel.normal.textColor;
            var transparentColor = new Color(opaqueColor.r, opaqueColor.g, opaqueColor.b, 0.6f);

            var style = new GUIStyle(EditorStyles.miniLabel)
            {              
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = transparentColor },
                margin = { right = 0 },
                border = { right = 0 },
                padding = { right = 0 },
            };

            return style;
        }
    }
}
