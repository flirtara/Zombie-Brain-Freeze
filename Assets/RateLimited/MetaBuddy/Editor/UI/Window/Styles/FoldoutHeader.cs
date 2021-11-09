using UnityEditor;
using UnityEngine;
using MetaBuddy.UI.Styles;

namespace MetaBuddy.UI.Window.Styles
{
    public class FoldoutHeader : IStyleFactory
    {
        private readonly Color _textColor;

        public FoldoutHeader(Color textColor)
        {
            _textColor = textColor;
        }

        public GUIStyle Create()
        {
            var style = StyleFactory.CreateWithTextColor(EditorStyles.foldoutPreDrop, _textColor);

            return style;
        }
    }
}
