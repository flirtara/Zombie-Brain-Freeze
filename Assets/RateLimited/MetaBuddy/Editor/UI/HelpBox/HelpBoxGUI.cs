using System.Collections.Generic;
using MetaBuddy.UI.HelpBox.Styles;
using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.HelpBox
{
    public class HelpBoxGUI
    {
        private readonly StyleCache _style = new StyleCache(new HelpBoxStyle());
        private IReadOnlyDictionary<MessageType, Texture> _iconsByType;

        private static IReadOnlyDictionary<MessageType, Texture> CreateIconsByType()
        {
            return new Dictionary<MessageType, Texture>()
            {
                {
                    MessageType.Info,
                    EditorGUIUtility.IconContent("console.infoicon").image
                },
                {
                    MessageType.Warning,
                    EditorGUIUtility.IconContent("console.warnicon").image
                },
                {
                    MessageType.Error,
                    EditorGUIUtility.IconContent("console.erroricon").image
                },
            };
        }

        private IReadOnlyDictionary<MessageType, Texture> IconsByType
        {
            get
            {
                _iconsByType = _iconsByType ?? CreateIconsByType();
                return _iconsByType;
            }
        }

        private Texture IconForMessageType(MessageType messageType)
        {
            if(IconsByType.TryGetValue(messageType, out Texture texture))
            {
                return texture;
            }

            return null;
        }

        public void Generate(string message, MessageType messageType)
        {
            EditorGUILayout.BeginVertical();

            var icon = IconForMessageType(messageType);
            var content = new GUIContent(message, icon);
            GUILayout.Label(content, _style.Get);

            EditorGUILayout.EndVertical();
        }
    }
}
