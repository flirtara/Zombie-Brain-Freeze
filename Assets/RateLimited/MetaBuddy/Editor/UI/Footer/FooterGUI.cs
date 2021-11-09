using MetaBuddy.App;
using MetaBuddy.UI.Footer.Styles;
using MetaBuddy.UI.Styles;
using MetaBuddy.Util;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.Footer
{
    public static class FooterGUI
    {
        private static readonly StyleCache _linkStyle = new StyleCache(new Link(false));
        private static readonly StyleCache _miniLinkStyle = new StyleCache(new Link(true));

        private static readonly StyleCache _versionStyle = new StyleCache(new Version(false));
        private static readonly StyleCache _miniVersionStyle = new StyleCache(new Version(true));

        private static readonly StyleCache _supportStyle = new StyleCache(new Support(false));
        private static readonly StyleCache _miniSupportStyle = new StyleCache(new Support(true));

        private static string _version;

        private class Labels
        {
            public static GUIContent ProductTitle = new GUIContent
            (
                Product.Name,
                "Visit the MetaBuddy home page."
            );

            public static GUIContent EmailSupport = new GUIContent
            (
                "Email",
                "Support via email."
            );

            public static GUIContent ChatSupport = new GUIContent
            (
                "Chat",
                "Support via Discord chat."
            );

            public static GUIContent GitHubSupport = new GUIContent
            (
                "GitHub",
                "Raise an issue on GitHub."
            );

            public static GUIContent FormSupport = new GUIContent
            (
                "Form",
                "Support via contact form."
            );
        }

        private static void GenerateSupport(bool miniLabel)
        {
            var supportStyle = miniLabel
                   ? _miniSupportStyle
                   : _supportStyle;

            var linkStyle = miniLabel
                    ? _miniLinkStyle
                    : _linkStyle;

            GUILayout.Label("Support & Feedback: ", supportStyle.Get);

            if (GUIUtil.LinkLabel(Labels.ChatSupport, linkStyle.Get))
            {
                Product.OpenChatSupport();
            }

            GUILayout.Label(" / ", supportStyle.Get);

            if (GUIUtil.LinkLabel(Labels.GitHubSupport, linkStyle.Get))
            {
                Product.OpenGitHubSupport();
            }

            GUILayout.Label(" / ", supportStyle.Get);

            if (GUIUtil.LinkLabel(Labels.EmailSupport, linkStyle.Get))
            {
                Product.OpenEmailSupport();
            }

            GUILayout.Label(" / ", supportStyle.Get);

            if (GUIUtil.LinkLabel(Labels.FormSupport, linkStyle.Get))
            {
                Product.OpenContactFormSupport();
            }
        }

        public static void Generate(bool miniLabel)
        {
            _version = _version ?? new VersionFetcher().Version;

            EditorGUILayout.BeginHorizontal();

                var linkStyle = miniLabel
                    ? _miniLinkStyle
                    : _linkStyle;

                if (GUIUtil.LinkLabel(Labels.ProductTitle, linkStyle.Get))
                {
                    Product.OpenHomePage();
                }

                var versionStyle = miniLabel
                    ? _miniVersionStyle
                    : _versionStyle;

                GUILayout.Label($"v{_version}", versionStyle.Get);

                GUILayout.FlexibleSpace();

                GenerateSupport(miniLabel);   

            EditorGUILayout.EndHorizontal();
        }
    }
}
