using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.HelpBox.Styles
{
    public class HelpBoxStyle : IStyleFactory
    {
        public GUIStyle Create()
        {
            int padding = StyleConstants.StandardPadding;
            var style = new GUIStyle(EditorStyles.helpBox)
            {
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
