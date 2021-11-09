using MetaBuddy.UI.Styles;
using UnityEngine;

namespace MetaBuddy.UI.Settings.Styles
{
    public class SettingsContainer : IStyleFactory
    {
        public GUIStyle Create()
        {
            var p = StyleConstants.StandardPadding;

            var style = new GUIStyle()
            {
                padding =
                {
                    left = p,
                    right = p,
                    top = p,
                    bottom = p
                }
            };

            return style;
        }
    }
}
