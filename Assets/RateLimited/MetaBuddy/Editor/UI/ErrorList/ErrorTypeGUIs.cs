using System;
using System.Collections.Generic;
using LibGit2Sharp;
using MetaBuddy.App;
using MetaBuddy.UI.ErrorType;
using MetaBuddyLib.Model.Errors;

namespace MetaBuddy.UI.ErrorList
{
    public class ErrorTypeGUIs
    {
        private static Uri ErrorTypeUri(string errorAnchor)
        {
            return new Uri(Product.ErrorDetailsUri, errorAnchor);
        }

        struct ErrorPattern
        {
            public ErrorPattern(AssetErrorType errorType, FileStatus? fileStatus) : this()
            {
                this.errorType = errorType;
                this.fileStatus = (errorType == AssetErrorType.CorruptMetaFile)
                    ? FileStatus.Unaltered :
                    fileStatus;
            }

            private readonly AssetErrorType errorType;
            private readonly FileStatus? fileStatus;
        }

        private readonly Dictionary<ErrorPattern, ErrorTypeGUI> _guisByType
            = new Dictionary<ErrorPattern, ErrorTypeGUI>()
        {
            {
                new ErrorPattern(AssetErrorType.MissingMetaFile, FileStatus.NewInIndex),
                new ErrorTypeGUI
                (
                    "Asset without meta",
                    ErrorTypeUri("#asset-added-without-adding-its-meta-file"),
                    "Suggested resolution:\n\nStage an add for this asset's meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingDirectoryMetaFile, FileStatus.NewInIndex),
                new ErrorTypeGUI
                (
                    "Directory without meta",
                    ErrorTypeUri("#directory-added-without-adding-its-meta-file"),
                    "Suggested resolution:\n\nStage an add for this directory's meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingContentFile, FileStatus.NewInIndex),
                new ErrorTypeGUI
                (
                    "Meta without asset",
                    ErrorTypeUri("#meta-file-added-without-adding-its-asset"),
                    "Suggested resolution:\n\nAdd the asset for this meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingMetaFile, FileStatus.DeletedFromIndex),
                new ErrorTypeGUI
                (
                    "Asset deleted without meta",
                    ErrorTypeUri("#asset-deleted-without-deleting-its-meta-file"),
                    "Suggested resolution:\n\nStage a delete for this asset's meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingDirectoryMetaFile, FileStatus.DeletedFromIndex),
                new ErrorTypeGUI
                (
                    "Directory deleted without meta",
                    ErrorTypeUri("#directory-deleted-without-deleting-its-meta-file"),
                    "Suggested resolution:\n\nStage a delete for this directory's meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingContentFile, FileStatus.DeletedFromIndex),
                new ErrorTypeGUI
                (
                    "Meta deleted without asset",
                    ErrorTypeUri("#meta-file-deleted-without-deleting-its-asset"),
                    "Suggested resolution:\n\nStage a delete for this meta file's asset.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingMetaFile, FileStatus.RenamedInIndex),
                new ErrorTypeGUI
                (
                    "Asset renamed without meta",
                    ErrorTypeUri("#asset-renamed-without-renaming-its-meta-file"),
                    "Suggested resolution:\n\nStage an add for this asset's meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.MissingContentFile, FileStatus.RenamedInIndex),
                new ErrorTypeGUI
                (
                    "Meta renamed without asset",
                    ErrorTypeUri("#meta-file-renamed-without-renaming-its-asset"),
                    "Suggested resolution:\n\nAdd that asset for this meta file.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.CorruptMetaFile, FileStatus.Unaltered),
                new ErrorTypeGUI
                (
                    "Corrupt meta file",
                    ErrorTypeUri("#corrupt-meta-file-contents"),
                    "Suggested resolution:\n\n" +
                        "Open the meta file in a text editor and look for lines containing invalid YAML.\n\n" +
                        "This is often caused by unresolved conflicts that look something like this:\n\n" +
                        "<<<<<<< HEAD:file.txt\n" +
                        "Content from head\n" +
                        "=======\n" +
                        "Your content" +
                        ">>>>>>> 77976\n\n" +
                        "Resolve the conflicted lines in the meta file to fix this error.",
                    "Learn more about this error"
                )
            },

            {
                new ErrorPattern(AssetErrorType.ChangeToGuid, FileStatus.ModifiedInIndex),
                new ErrorTypeGUI
                (
                    "Modified GUID",
                    ErrorTypeUri("#modified-guid-in-meta-file"),
                    "Suggested resolution:\n\n" +
                     "Revert the change to the GUID in this meta file.",
                    "Learn more about this error"
                )
            }
        };

        public ErrorTypeGUI FindForError(IErrorDetails error)
        {
            var errorPattern = new ErrorPattern(error.ErrorType, error.FileStatus);

            if(_guisByType.ContainsKey(errorPattern))
            {
                return _guisByType[errorPattern];
            }

            return null;
        }
    }
}

