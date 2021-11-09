using UnityEngine;

namespace MetaBuddy.UI.Styles
{
    public static class StyleFactory
    {
        public static GUIStyle CreateWithTextColor(GUIStyle baseStyle, Color textColor)
        {
            var style = new GUIStyle(baseStyle);

            style.normal.textColor = textColor;
            style.onNormal.textColor = textColor;
            style.hover.textColor = textColor;
            style.onHover.textColor = textColor;
            style.focused.textColor = textColor;
            style.onFocused.textColor = textColor;
            style.active.textColor = textColor;
            style.onActive.textColor = textColor;

            return style;
        }

        public static GUIStyle CreateBackground
        (
            Color backgroundColor,
            Color rolloverBackgroundColor
        )
        {
            var style = new GUIStyle(GUIStyle.none)
            {
                normal = { background = GUIUtil.CreateColorTexture(backgroundColor) },
                hover = { background = GUIUtil.CreateColorTexture(rolloverBackgroundColor) }               
            };

            return style;           
        }
    }
}
