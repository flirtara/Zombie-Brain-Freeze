using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.Footer.Styles
{
    public class Link : IStyleFactory
    {
        private readonly bool _mini;

        public Link(bool mini)
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
                alignment = TextAnchor.UpperRight,
                padding = { right = 0, left = 0 },
                border = { right = 0, left = 0 },
                margin = { right = 0, left = 0 },

                normal = { textColor = StyleConstants.LinkColor },
                hover = { textColor = StyleConstants.LinkColor }
            };
       
            return style;
        }
    }
}
