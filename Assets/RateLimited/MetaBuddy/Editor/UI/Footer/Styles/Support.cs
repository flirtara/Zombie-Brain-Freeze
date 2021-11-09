using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.Footer.Styles
{
    public class Support : IStyleFactory
    {
        private readonly bool _mini;

        public Support(bool mini)
        {
            _mini = mini;
        }

        public GUIStyle Create()
        {
            var baseStyle = _mini
                ? EditorStyles.miniLabel
                : EditorStyles.label;

            var style = new GUIStyle(baseStyle)
            {
                alignment = TextAnchor.UpperLeft,
                padding = { left = 0, right = 0 },
                border = { left = 0, right = 0 },
                margin = { left = 0, right = 0}
            };

            return style;
        }
    }
}
