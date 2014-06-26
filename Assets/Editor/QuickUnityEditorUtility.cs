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
        /// Converts absolute path to relative path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToRelativePath(string absolutePath)
        {
            return absolutePath.Substring(absolutePath.IndexOf("Assets/"));
        }
    }
}