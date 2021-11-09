using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorRow.Styles
{
    public class ErrorFilename : IStyleFactory
    {
        public GUIStyle Create()
        {
            var style = new GUIStyle(EditorStyles.miniLabel)
            {              
                alignment = TextAnchor.MiddleLeft,       
            };

            return style;
        }
    }
}
