using MetaBuddyLib.Model.Errors;
using System.Linq;
using MetaBuddy.UI.ErrorRow;
using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI.ErrorList
{
    public class ErrorListGUI 
    {
        private readonly ErrorTypeGUIs _errorGUIs = new ErrorTypeGUIs();
        private readonly ErrorRowGUI _rowGenerator = new ErrorRowGUI();
        private Vector2 _scrollPosition = Vector2.zero;

        private void GenerateRows
        (
            AssetErrorCollection errors,
            string basePath,
            string projectPath
        )
        {
            var errorArray = errors.Errors.ToArray();

            var initialError = (errorArray.Length > 1)
                ? errorArray[1]
                : null;

            _rowGenerator.Reset(basePath, projectPath, initialError);

            bool isOdd = false;
            foreach (var error in errors.Errors)
            {
                var errorTypeGUI = _errorGUIs.FindForError(error);

                _rowGenerator.Generate
                (
                    error,
                    errorTypeGUI,
                    isOdd
                );

                isOdd = !isOdd;
            }
        }

        public void Generate
        (
            AssetErrorCollection errors,
            string basePath,
            string projectPath
        )
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            GenerateRows(errors, basePath, projectPath);

            EditorGUILayout.EndScrollView();          
        }
    }
}
