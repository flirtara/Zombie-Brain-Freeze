using System;

namespace MetaBuddy.Util
{
    public class CommonPrefixFinder
    {
        public string ExtractCommonAndRemainer(string a, string b, out string bRemainder)
        {
            var minLength = Math.Min(a.Length, b.Length);

            int prefixLength = 0;
            int lastSeperatorLength = -1;

            for (int i = 0; i < minLength && a[i] == b[i]; i++)
            {
                prefixLength++;

                if (a[i] == '/')
                {
                    lastSeperatorLength = prefixLength;
                }
            }

            if (prefixLength > 0)
            {
                if (lastSeperatorLength != -1)
                {
                    prefixLength = lastSeperatorLength;
                }

                var prefix = a.Substring(0, prefixLength);

                bRemainder = (prefixLength < b.Length)
                    ? b.Substring(prefixLength)
                    : string.Empty;

                return prefix;
            }

            bRemainder = null;
            return null;
        }  
    }
}

