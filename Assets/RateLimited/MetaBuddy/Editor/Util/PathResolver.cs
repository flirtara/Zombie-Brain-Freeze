using System.IO;
using UnityEngine;

namespace MetaBuddy.Util
{
    public class PathResolver
    {
        private readonly string _projectRelativeBasePath;

        public static PathResolver Create(string packageName, ScriptableObject scriptObj, string baseRelativePath, int scriptDepthBelowBase = 0)
        {
            var scriptDirPath = PathHelpers.GetScriptDirectoryPath(scriptObj);
            var baseDirPath = PathHelpers.GetParentPath(scriptDirPath, scriptDepthBelowBase);

            var projectRelativePath = PathHelpers.GetProjectRelativePathWithinPackage(packageName, baseDirPath);

            var projectRelativeBasePath = (baseRelativePath != null)
                ? Path.Combine(projectRelativePath, baseRelativePath)
                : projectRelativePath;
              
            return new PathResolver(projectRelativeBasePath);
        }

        public PathResolver(string projectRelativeBasePath)
        {
            _projectRelativeBasePath = projectRelativeBasePath;
        }

        public string ResolvePath(string relativePath)
        {
            var path = Path.Combine(_projectRelativeBasePath, relativePath);
            return path;
        }

        public string ProjectRelativeBasePath
        {
            get
            {
                return _projectRelativeBasePath;
            }
        }
    }
}
