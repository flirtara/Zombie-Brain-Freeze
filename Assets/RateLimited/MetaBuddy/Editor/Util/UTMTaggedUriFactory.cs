using System;

namespace MetaBuddy.Util
{
    public class UTMTaggedUriFactory
    {
        private readonly string utmTags;

        public UTMTaggedUriFactory(string utmTags)
        {
            this.utmTags = utmTags;
        }

        public Uri Create(Uri baseUri, string relativeUri)
        {
            return new Uri(baseUri, $"{relativeUri}?{utmTags}");
        }

        public Uri Create(Uri baseUri)
        {
            return new Uri(baseUri, $"?{utmTags}");
        }
    }
}
