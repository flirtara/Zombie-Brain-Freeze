using System;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI
{
    public static class GUIUtil
    {
        private static GUIContent _helpIcon;
        private static GUIContent _gearIcon;

        public static bool LinkLabel(GUIContent label, GUIStyle linkStyle, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, linkStyle, options);

            if (Event.current.type == EventType.Repaint && position.Contains(Event.current.mousePosition))
            {
                Handles.BeginGUI();
                var saveColor = Handles.color;
                Handles.color = linkStyle.normal.textColor;

                Handles.DrawLine
                    (
                        new Vector3(position.xMin, position.yMax),
                        new Vector3(position.xMax, position.yMax)
                    );

                Handles.color = saveColor;
                Handles.EndGUI();
            }

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, linkStyle);
        }

        private static bool ToolbarIcon(GUIContent content, string toolTip)
        {
            var iconButtonStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("IconButton");

            var contentWithToolTip = new GUIContent(content);
            content.tooltip = toolTip;

            return GUILayout.Button(contentWithToolTip, iconButtonStyle);
        }

        private static GUIContent EditorHelpIcon
        {
            get
            {
                _helpIcon = _helpIcon ?? EditorGUIUtility.IconContent("_help");
                return _helpIcon;
            }
        }

        private static GUIContent EditorGearIcon
        {
            get
            {
                _gearIcon = _gearIcon ?? EditorGUIUtility.IconContent("_Popup");
                return _gearIcon;
            }
        }


        public static bool HelpIcon()
        {
            return ToolbarIcon(EditorHelpIcon, "Open reference for MetaBuddy");
        }

        public static bool GearIcon()
        {
            return ToolbarIcon(EditorGearIcon, "Open MetaBuddy Settings");
        }

        public static Texture2D CreateColorTexture(Color color)
        {
            var singlePixel = new Color[1];
            singlePixel[0] = color;

            var texture = new Texture2D(1, 1);
            texture.SetPixels(singlePixel);
            texture.Apply();

            return texture;
        }

        public static string FileManagerName
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                        return "Finder";

                    case RuntimePlatform.WindowsEditor:
                        return "Explorer";

                    default:
                        return "File Manager";
                }
            }
        }    
    }
}
