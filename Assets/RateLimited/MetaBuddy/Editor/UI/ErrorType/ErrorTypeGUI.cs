using System;
using MetaBuddy.UI.ErrorType.Styles;
using MetaBuddy.UI.Styles;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorType
{
    public class ErrorTypeGUI
    {
        private readonly string _name;
        private readonly string _errorLabelToolTip;
        private readonly string _helpIconToolTip;
        private GUIContent _helpIconContent;
        private GUIContent _errorLabelContent;

        private readonly StyleCache _errorLabelStyle = new StyleCache(new ErrorLabel());
        private readonly StyleCache _helpButtonStyle = new StyleCache(new ErrorHelpButton());

        public ErrorTypeGUI
        (
            string name,
            Uri helpUri,
            string errorLabelToolTip,
            string helpIconToolTip
        )
        {
            _name = name;
            HelpUri = helpUri;
            _errorLabelToolTip = errorLabelToolTip;
            _helpIconToolTip = helpIconToolTip;
        }

        public Uri HelpUri { get; }

        private GUIContent CreateHelpIconContent()
        {
            var icon = EditorGUIUtility.IconContent("_help");

            var content = new GUIContent(icon)
            {
                tooltip = _helpIconToolTip
            };

            return content;
        }

        private GUIContent HelpIconContent
        {
            get
            {
                _helpIconContent = _helpIconContent ?? CreateHelpIconContent();
                return _helpIconContent;
            }
        }

        private GUIContent CreateErrorLabelContent()
        {
            var content = new GUIContent(_name)
            {
                tooltip = _errorLabelToolTip
            };

            return content;
        }

        private GUIContent ErrorLabelContent
        {
            get
            {
                _errorLabelContent = _errorLabelContent ?? CreateErrorLabelContent();
                return _errorLabelContent;
            }
        }

        private bool GenerateHelpButton()
        {          
            return GUILayout.Button(HelpIconContent, _helpButtonStyle.Get);
        }

        public bool Generate()
        {
            GUILayout.Label(ErrorLabelContent, _errorLabelStyle.Get);

            var helpClicked = GenerateHelpButton();

            if(helpClicked && Event.current.button == 0)
            {
                Application.OpenURL(HelpUri.ToString());
            }

            return helpClicked;
        }
    }
}
