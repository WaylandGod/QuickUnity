using System.Collections;
using UnityEditor;
using UnityEngine;

namespace QuickUnityEditor.Assets
{
    /// <summary>
    /// Class AssetBundleEditor to edit AssetBundle files.
    /// </summary>
    public sealed class AssetBundleEditor : Editor
    {
        /// <summary>
        /// File extension of AssetBundle.
        /// </summary>
        private const string ASSET_BUNDLE_FILE_EXT = ".unity3d";

        /// <summary>
        /// Build AssetBundle for Windows.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundles (Windows)")]
        public static void BuildAssetBundlesForWin()
        {
            BuildAssetBundles();
        }

        /// <summary>
        /// Build all in one AssetBundle file for Windows.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundle All In One (Windows)")]
        public static void BuildAssetBundleAllInOneForWin()
        {
            BuildAssetBundles(false);
        }

        /// <summary>
        /// Build AssetBundle file for WebPlayer.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundles (WebPlayer)")]
        public static void BuildAssetBundleForWeb()
        {
            BuildAssetBundles(true, BuildTarget.WebPlayer);
        }

        /// <summary>
        /// Build all in one AssetBundle file for WebPlayer.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundle All In One (WebPlayer)")]
        public static void BuildAssetBundleAllInOneForWeb()
        {
            BuildAssetBundles(false, BuildTarget.WebPlayer);
        }

        /// <summary>
        /// Build AssetBundle file for Android.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundles (Android)")]
        public static void BuildAssetBundleForAndroid()
        {
            BuildAssetBundles(true, BuildTarget.Android);
        }

        /// <summary>
        /// Build all in one AssetBundle file for Andorid.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundle All In One (Andorid)")]
        public static void BuildAssetBundleAllInOneForAndroid()
        {
            BuildAssetBundles(false, BuildTarget.Android);
        }

        /// <summary>
        /// Build AssetBundle file for iOS.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundles (iOS)")]
        public static void BuildAssetBundleForIOS()
        {
            BuildAssetBundles(true, BuildTarget.iPhone);
        }

        /// <summary>
        /// Build all in one AssetBundle file for iOS.
        /// </summary>
        [MenuItem("QuickUnity/Assets/Build AssetBundle All In One (iOS)")]
        public static void BuildAssetBundleAllInOneForIOS()
        {
            BuildAssetBundles(false, BuildTarget.iPhone);
        }

        /// <summary>
        /// Builds the asset bundles.
        /// </summary>
        /// <param name="isStandalone">if set to <c>true</c>, each AssetBundle file has an Object; else All Objects in one AssetBundle file.</param>
        /// <param name="targetPlatform">The target platform.</param>
        private static void BuildAssetBundles(bool isStandalone = true, BuildTarget targetPlatform = BuildTarget.StandaloneWindows)
        {
            string path = EditorUtility.SaveFolderPanel("Save AssetBundles", "", "");

            if (string.IsNullOrEmpty(path))
                return;

            Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            string pathName = "";

            if (selectedAssets.Length == 0)
            {
                Debug.LogWarning("Please select valid assets !");
                return;
            }

            if (isStandalone)
            {
                // Pack each asset into one AssetBundle file.
                foreach (Object obj in selectedAssets)
                {
                    pathName = path + "/" + obj.name + ASSET_BUNDLE_FILE_EXT;

                    if (BuildPipeline.BuildAssetBundle(obj, null, pathName, BuildAssetBundleOptions.CollectDependencies, targetPlatform))
                    {
                        Debug.Log("The AssetBundle file generated: " + pathName);
                    }
                }
            }
            else
            {
                // Pack all assets into one AssetBundle file.
                pathName = path + "/All" + ASSET_BUNDLE_FILE_EXT;
                if (BuildPipeline.BuildAssetBundle(null, selectedAssets, pathName, BuildAssetBundleOptions.CollectDependencies, targetPlatform))
                {
                    Debug.Log("The AssetBundle file generated: " + pathName);
                }
            }

            // Refresh asset database.
            AssetDatabase.Refresh();
        }
    }
}