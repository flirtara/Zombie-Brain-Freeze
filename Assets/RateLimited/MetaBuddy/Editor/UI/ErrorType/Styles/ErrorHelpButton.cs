using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorType.Styles
{
    public class ErrorHelpButton : IStyleFactory
    {
        public GUIStyle Create()
        {
            var iconButtonStyle = EditorGUIUtility
                .GetBuiltinSkin(EditorSkin.Inspector)
                .FindStyle("IconButton");          

            var style = new GUIStyle(iconButtonStyle)
            {
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
