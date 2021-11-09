using MetaBuddyLib.Util;

namespace MetaBuddy.Util
{
    public class VersionFetcher
    {
        public string Version => AssemblyInfo.GetInformationalVersion(GetType().Assembly);
    }
}
