using MetaBuddy.UI.Styles;
using UnityEngine;

namespace MetaBuddy.UI.Footer.Styles
{
    public class FooterContainer : IStyleFactory
    {
        public GUIStyle Create()
        {
            var style = new GUIStyle()
            {
                padding =
                {
                    left = StyleConstants.StandardPadding,
                    right = StyleConstants.StandardPadding,
                    top = 5,
                    bottom = 5
                }
            };

            return style;       
        }
    }
}
