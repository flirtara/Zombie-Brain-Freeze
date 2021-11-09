using System.IO;
using MetaBuddyLib.Config;
using UnityEngine;

namespace MetaBuddy.Settings
{
    public static class DefaultConfigFactory
    {
        public static ConfigModel Create()
        {
            var projectPath = new DirectoryInfo(Path.GetDirectoryName(Application.dataPath));
            var defaultConfig = ConfigModel.CreateWithDefaults(projectPath);

            defaultConfig.CheckCommand = CheckCommand.Changes;

            return defaultConfig;
        }
    }
}