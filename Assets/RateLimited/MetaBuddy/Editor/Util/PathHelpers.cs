using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.Util
{
    public class PathHelpers
    {
        public static string ProjectPath
        {
            get
            {
                return Path.GetDirectoryName(Application.dataPath);
            }
        }

        public static string GetScriptPath(ScriptableObject scriptObj)
        {
            var monoScript = MonoScript.FromScriptableObject(scriptObj);
            return AssetDatabase.GetAssetPath(monoScript);
        }

        public static string GetScriptDirectoryPath(ScriptableObject scriptObj)
        {
            return Directory.GetParent(GetScriptPath(scriptObj)).FullName;
        }

        private static readonly string DirectorySeparatorString = Path.DirectorySeparatorChar.ToString();
        private static readonly string AltDirectorySeparatorString = Path.AltDirectorySeparatorChar.ToString();
        private static readonly char[] AllDirectorySeparators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        public static bool EndsWithDirectorySeperator(string path)
        {
            return path.EndsWith(DirectorySeparatorString) || path.EndsWith(AltDirectorySeparatorString);
        }

        public static bool StartsWithSeparator(string path)
        {
            return path.StartsWith(DirectorySeparatorString) || path.StartsWith(AltDirectorySeparatorString);
        }

        public static string WithTrailingSeparator(string path)
        {
            if (!EndsWithDirectorySeperator(path))
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        public static string[] SplitPath(string path)
        {
            var parts = path.Split(AllDirectorySeparators, StringSplitOptions.None);

            return parts;
        }

        private static string PathFromParts(string[] parts, int startIndex, int numParts)
        {
            var includeParts = new ArraySegment<string>(parts, startIndex, numParts);
            return string.Join(DirectorySeparatorString, includeParts.ToArray());
        }

        public static string MakeRelativePath(string basePath, string path)
        {
            var baseParts = SplitPath(basePath);
            var numBaseParts = baseParts.Length;

            var pathParts = SplitPath(path);
            var numPathParts = pathParts.Length;

            if (numPathParts >= numBaseParts)
            {
                for (int i = 0; i < numBaseParts; i++)
                {
                    if (pathParts[i] != baseParts[i])
                    {
                        return null;
                    }
                }

                var numRelativeParts = numPathParts - numBaseParts;
                return PathFromParts(pathParts, numBaseParts, numRelativeParts);
            }

            return null;
        }

        public static string GetProjectRelativePath(string itemPath)
        {
            return MakeRelativePath(ProjectPath, itemPath);
        }

        public static bool HasBasePath(string basePath, string path)
        {
            var baseParts = SplitPath(basePath);
            var pathParts = SplitPath(path); 

            if(pathParts.Length >= baseParts.Length)
            {
                for (int i = 0; i < baseParts.Length; i++)
                {
                    if (pathParts[i] != baseParts[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private static readonly string PackageCachePath = Path.Combine("Library", "PackageCache");

        public static bool IsPathInPackageCache(string path)
        {
            return HasBasePath(PackageCachePath, path);
        }

        public static readonly string PackagesPath = "Packages";

        public static string GetProjectRelativePathWithinPackage(string packageName, string itemPath)
        {
            var projectRelativePath = GetProjectRelativePath(itemPath);

            if(IsPathInPackageCache(projectRelativePath))
            {
                var leadingDirsToStrip = 3;
                var packageRoot = GetChildPath(projectRelativePath, leadingDirsToStrip);

                return Path.Combine(PackagesPath, packageName, packageRoot);
            }

            return projectRelativePath;
        }

        public static string GetParentPath(string path, int childLevelsToRemove = 1)
        {
            var pathParts = SplitPath(path);

            var numPathParts = pathParts.Length;

            if(numPathParts >= childLevelsToRemove)
            {
                return PathFromParts(pathParts, 0, numPathParts - childLevelsToRemove);
            }

            throw new ArgumentException($"Path {path} has less parts ({numPathParts}) than the number of child parts be removed ({childLevelsToRemove})");
        }

        public static string GetChildPath(string path, int parentLevelsToRemove = 1)
        {
            var pathParts = SplitPath(path);

            var numPathParts = pathParts.Length;

            if (numPathParts >= parentLevelsToRemove)
            {
                return PathFromParts(pathParts, parentLevelsToRemove, numPathParts - parentLevelsToRemove);
            }

            throw new ArgumentException($"Path {path} has less parts ({numPathParts}) than the number parent parts to be removed ({parentLevelsToRemove})");
        }
    }
}

