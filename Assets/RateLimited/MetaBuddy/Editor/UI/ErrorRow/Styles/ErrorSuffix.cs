using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorRow.Styles
{
    public class ErrorSuffix : IStyleFactory
    {
        public GUIStyle Create()
        {
            var style = new GUIStyle(EditorStyles.miniLabel)
            {              
                alignment = TextAnchor.MiddleLeft,
                margin = { left = 0 },
                border = { left = 0 },
                padding = { left = 0 },
            };

            return style;
        }
    }
}
