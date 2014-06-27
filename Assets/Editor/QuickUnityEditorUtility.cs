using System.Collections;
using UnityEngine;

namespace QuickUnityEditor
{
    /// <summary>
    /// A utility class to help Editor of QuickUnity. This class cannot be inherited.
    /// </summary>
    public sealed class QuickUnityEditorUtility
    {
        /// <summary>
        /// The error message about file not be found.
        /// </summary>
        private const string ERROR_FILE_NOT_FOUND = "File $fileName$ could not be found !";

        /// <summary>
        /// The warning message about path is null or empty
        /// </summary>
        private const string WARNING_PATH_NULL_OR_EMPTY = "The path is invalid, please check if your path string is not null or not empty";

        /// <summary>
        /// The error message about invalid path
        /// </summary>
        private const string ERROR_INVALID_PATH = "The path is invalid, please check if your path is in the directory Assets !";

        /// <summary>
        /// Converts absolute path to relative path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToRelativePath(string absolutePath)
        {
            return absolutePath.Substring(absolutePath.IndexOf("Assets/"));
        }

        /// <summary>
        /// Checks the asset file path.
        /// </summary>
        /// <param name="path">The path of asset file.</param>
        /// <returns><c>true</c> if the path is ok, <c>false</c> otherwise.</returns>
        public static bool CheckAssetFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning(WARNING_PATH_NULL_OR_EMPTY);
                return false;
            }

            if (path.IndexOf("Assets/") == -1)
            {
                Debug.LogError(ERROR_INVALID_PATH);
                return false;
            }

            return true;
        }
    }
}