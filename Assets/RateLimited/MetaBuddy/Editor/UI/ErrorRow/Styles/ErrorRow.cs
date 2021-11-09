using MetaBuddy.UI.Styles;
using UnityEngine;

namespace MetaBuddy.UI.ErrorRow.Styles
{
    public class ErrorRow : IStyleFactory
    {
        private readonly Color _backgroundColor;
        private readonly Color _rolloverBackgroundColor;

        public ErrorRow(Color backgroundColor, Color rolloverBackgroundColor)
        {
            _backgroundColor = backgroundColor;
            _rolloverBackgroundColor = rolloverBackgroundColor;
        }

        public GUIStyle Create()
        {
            return StyleFactory.CreateBackground(_backgroundColor, _rolloverBackgroundColor);
        }
    }
}
