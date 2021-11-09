using UnityEngine;
using MetaBuddyLib.Model.Errors;
using UnityEditor;
using MetaBuddy.UI.Styles;
using MetaBuddy.UI.ErrorType;
using MetaBuddy.UI.ErrorRow.Styles;
using System.IO;
using MetaBuddy.Util;
using MetaBuddyLib.Util;

namespace MetaBuddy.UI.ErrorRow
{
    public class ErrorRowGUI 
    {
        private readonly StyleCache _oddStyle = new StyleCache
        (
            new Styles.ErrorRow
            (
                StyleConstants.StrongMidGrey,
                StyleConstants.RowHighlightColor
            )
        );

        private readonly StyleCache _evenStyle = new StyleCache
        (
            new Styles.ErrorRow
            (
                StyleConstants.WeakMidGrey,
                StyleConstants.RowHighlightColor
            )
        );
        private readonly StyleCache _errorPrefixStyle = new StyleCache(new ErrorPrefix());
        private readonly StyleCache _errorSuffixStyle = new StyleCache(new ErrorSuffix());
        private readonly StyleCache _errorFilenameStyle = new StyleCache(new ErrorFilename());
        private readonly CommonPrefixFinder _prefixFinder = new CommonPrefixFinder();

        private string _basePath;
        private string _projectPath;
        private string _prevErrorProjectPath;

        private bool GenerateSplitPathGUI(string errorFilePosixPath, string prevErrorFilePosixPath)
        {
            if (prevErrorFilePosixPath != null)
            {
                string uniquePathPart;

                var prefix = _prefixFinder.ExtractCommonAndRemainer
                (
                    prevErrorFilePosixPath,
                    errorFilePosixPath,
                    out uniquePathPart
                );

                if (prefix != null)
                {
                    GUILayout.Label(prefix, _errorPrefixStyle.Get);
                    GUILayout.Label(uniquePathPart, _errorSuffixStyle.Get);
                }

                return true;
            }

            return false;
        }

        private void GeneratePathGUI(string errorFilePosixPath, string prevErrorFilePosixPath)
        {
            if(!GenerateSplitPathGUI(errorFilePosixPath, prevErrorFilePosixPath))
            {
                GUILayout.Label(errorFilePosixPath, _errorFilenameStyle.Get);
            }
        }

        private string GetFullPathForError(IErrorDetails error)
        {
            return Path.Combine(_basePath, error.FilePosixPath);
        }

        private string GetProjectPathForError(IErrorDetails error, string fullPath)
        {
            return PathHelpers.MakeRelativePath(_projectPath, fullPath);
        }

        private string GetProjectPathForError(IErrorDetails error)
        {
            return GetProjectPathForError(error, GetFullPathForError(error));
        }

        public void Reset
        (
            string basePath,
            string projectPath,
            IErrorDetails initialError
        )
        {
            _basePath = basePath;
            _projectPath = projectPath;

            if (initialError != null)
            {
                _prevErrorProjectPath = GetProjectPathForError(initialError);
            }
            else
            {
                _prevErrorProjectPath = null;
            }
        }

        private void GenerateHoverHighlight(Event currentEvent, Rect rowRect)
        {
            EditorGUIUtility.AddCursorRect(rowRect, MouseCursor.Link);

            if (currentEvent.type == EventType.MouseMove && rowRect.Contains(Event.current.mousePosition))
            {
                if (EditorWindow.mouseOverWindow != null)
                {
                    EditorWindow.mouseOverWindow.Repaint();
                }
            }
        }

        private void GenerateContextMenu
        (
            Event current,
            Rect rowRect,
            string fullPath,
            IErrorDetails error,
            string projectRelativePath,
            ErrorTypeGUI errorTypeGUI
        )
        {
            var menuOpenClick = current.type == EventType.ContextClick
                || current.type == EventType.MouseDown;

            if (menuOpenClick && rowRect.Contains(current.mousePosition))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent($"Reveal in {GUIUtil.FileManagerName}"), false, ShowInFileManager, fullPath);
                menu.AddItem(new GUIContent("Highlight in project"), false, HighlightInEditor, projectRelativePath);
                menu.AddItem(new GUIContent("Copy path to clipboard/Repository relative"), false, CopyFilenameToClipboard, error.FilePosixPath);
                menu.AddItem(new GUIContent("Copy path to clipboard/Project relative"), false, CopyFilenameToClipboard, projectRelativePath);
                menu.AddItem(new GUIContent("Copy path to clipboard/Full"), false, CopyFilenameToClipboard, fullPath);
                menu.AddItem(new GUIContent($"How to fix..."), false, LearnMoreAboutError, errorTypeGUI.HelpUri);
                menu.ShowAsContext();

                current.Use();
            }
        }

        public void Generate
        (           
            IErrorDetails error,
            ErrorTypeGUI errorTypeGUI,
            bool isOdd
        )
        {
            var backgroundStyle = isOdd
                ? _oddStyle.Get
                : _evenStyle.Get;

            EditorGUILayout.BeginHorizontal(backgroundStyle);

            var fullPath = GetFullPathForError(error);
            var projectRelativePath = GetProjectPathForError(error, fullPath);

            GeneratePathGUI(projectRelativePath, _prevErrorProjectPath);
            GUILayout.FlexibleSpace();
            errorTypeGUI.Generate();

            EditorGUILayout.EndHorizontal();

            var rowRect = GUILayoutUtility.GetLastRect();
            var currentEvent = Event.current;

            GenerateHoverHighlight(currentEvent, rowRect);

            GenerateContextMenu(
                currentEvent,
                rowRect,
                fullPath,
                error,
                projectRelativePath,
                errorTypeGUI
            );

            _prevErrorProjectPath = projectRelativePath;
        }

        private void CopyFilenameToClipboard(object filename)
        {
            GUIUtility.systemCopyBuffer = filename as string;
        }

        private void ShowInFileManager(object fullPath)
        {
            EditorUtility.RevealInFinder(fullPath as string);
        }

        private void LearnMoreAboutError(object helpUri)
        {
            Application.OpenURL(helpUri.ToString());
        }

        private void HighlightInEditor(object projectPathObj)
        {
            var projectPath = projectPathObj as string;

            var assetPath = UnityAssetFile.IsMetaFile(projectPath)
                ? UnityAssetFile.AssetPathFromMetaFilePath(projectPath)
                : projectPath;

            var resource = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (resource)
            {
                EditorGUIUtility.PingObject(resource);
            }
        }
    }
}
