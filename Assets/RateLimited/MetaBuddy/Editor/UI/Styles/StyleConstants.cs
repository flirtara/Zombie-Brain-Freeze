using MetaBuddy.Util;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.Styles
{
    public static class StyleConstants
    {
        public static Color DarkGreen = ColorExtensions.FromHex("#0D8050");
        public static Color BrightGreen = ColorExtensions.FromHex("#0F9960");

        public static Color DarkRed = ColorExtensions.FromHex("#C23030");
        public static Color BrightRed = ColorExtensions.FromHex("#DB3737");

        public static Color LinkColor = ColorExtensions.FromHex("#1F68B0");

        public static Color OKColor
        {
            get { return EditorGUIUtility.isProSkin ? BrightGreen : DarkGreen; }
        }

        public static Color ErrorColor
        {
            get { return EditorGUIUtility.isProSkin ? BrightRed : DarkRed; }
        }

        public static readonly int StandardPadding = 10;

        public static readonly Color WeakMidGrey = new Color(0.5f, 0.5f, 0.5f, 0.0f);
        public static readonly Color StrongMidGrey = new Color(0.5f, 0.5f, 0.5f, 0.1f);

        public static readonly Color WeakDarkGrey = new Color(0.0f, 0.0f, 0.0f, 0.1f);
        public static readonly Color WeakLightGrey = new Color(1.0f, 1.0f, 1.0f, 0.1f);

        public static Color RowHighlightColor
        {
            get { return EditorGUIUtility.isProSkin ? WeakLightGrey : WeakDarkGrey; }
        }

        public static readonly float TwoThirdsOpacity = 0.66f;
    }
}
