using System;
using MetaBuddy.Util;
using UnityEngine;

namespace MetaBuddy.App
{
    public static class Product
    {
        public const string Name = "MetaBuddy";
        public static string Title => $"{Name} for Unity";
        public static string TitleWithVersion => $"{Title} version {new VersionFetcher().Version}";
        public static string Copyright => $"Copyright(C) Rate Limited. All rights reserved.";

        private static Uri HostUri => new Uri("https://ratelimited.io");
        private static Uri BaseUri => new Uri(HostUri, "metabuddy/");

        private static string utmTags = "utm_source=metabuddy_asset&utm_medium=help_link&utm_campaign=default";
        private static UTMTaggedUriFactory uriFactory = new UTMTaggedUriFactory(utmTags);

        public static Uri HomePageUri => uriFactory.Create(BaseUri);
        public static Uri DocumentationUri => uriFactory.Create(BaseUri, "docs");
        public static Uri ErrorDetailsUri => uriFactory.Create(BaseUri, "docs/errors");

        public static Uri ChatSupportUri => new Uri("https://discord.gg/Rpw89E9eR9");
        public static string EmailSupportUrl = "mailto:support@ratelimited.io?subject=MetaBuddy%20support%20request%20(from%20MetaBuddy%20Editor%20Extension)";
        public static Uri ContactFormSupportUri => uriFactory.Create(HostUri, "contact");
        public static Uri GitHubSupportUri => new Uri("https://github.com/rate-limited/metabuddy/issues");


        public static string Banner => $"{TitleWithVersion}\n{Copyright} {HostUri}";
        public static string SupportEmail => $"support@ratelimited.io";

        public static string PackageName = "com.ratelimited.metabuddy";

        public static void OpenHomePage()
        {
            Application.OpenURL(HomePageUri.ToString());
        }

        public static void OpenDocumentationPage()
        {
            Application.OpenURL(DocumentationUri.ToString());
        }

        public static void OpenChatSupport()
        {
            Application.OpenURL(ChatSupportUri.ToString());
        }

        public static void OpenGitHubSupport()
        {
            Application.OpenURL(GitHubSupportUri.ToString());
        }

        public static void OpenEmailSupport()
        {
            Application.OpenURL(EmailSupportUrl);
        }

        public static void OpenContactFormSupport()
        {
            Application.OpenURL(ContactFormSupportUri.ToString());
        }
    }
}
