/*************************************************************************************
     * Class Name:  AssetBundleEditor
     * Namespace:
     * Author：       cosmos53076@163.com
     * Description:  Editor functions for AssetBundle objects
    *************************************************************************************/

using System.Collections;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditor : Editor
{
    private const string ASSET_BUNDLE_FILE_EXT = ".assetbundle";

    [MenuItem("QuickUnity/AssetBundle/Create AssetBundles (Windows)")]
    public static void CreateAssetBundlesForWin()
    {
        CreateAssetBundlesByParams();
    }

    [MenuItem("QuickUnity/AssetBundle/Create AssetBundles (iPhone)")]
    public static void CreateAssetBundlesForIPhone()
    {
        CreateAssetBundlesByParams(true, BuildTarget.iPhone);
    }

    [MenuItem("QuickUnity/AssetBundle/Create AssetBundles (Android)")]
    public static void CreateAssetBundlesForAndroid()
    {
        CreateAssetBundlesByParams(true, BuildTarget.Android);
    }

    [MenuItem("QuickUnity/AssetBundle/Create AssetBundle All In One (Windows)")]
    public static void CreateAssetBundleAllInOneForWin()
    {
        CreateAssetBundlesByParams(false);
    }

    [MenuItem("QuickUnity/AssetBundle/Create AssetBundle All In One (iPhone)")]
    public static void CreateAssetBundleAllInOneForIPhone()
    {
        CreateAssetBundlesByParams(false, BuildTarget.iPhone);
    }

    [MenuItem("QuickUnity/AssetBundle/Create AssetBundle All In One (Android)")]
    public static void CreateAssetBundleAllInOneForAndroid()
    {
        CreateAssetBundlesByParams(false, BuildTarget.Android);
    }

    private static void CreateAssetBundlesByParams(bool isStandalone = true, BuildTarget targetPlatform = BuildTarget.StandaloneWindows)
    {
        Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string pathName = "";

        if (selectedAssets.Length == 0)
        {
            Debug.LogWarning("Please select valid assets !");
            return;
        }

        if (isStandalone)
        {
            //pack each asset into one assetbundle file
            foreach (Object obj in selectedAssets)
            {
                pathName = CommonPath.STREAMING_ASSETS + obj.name + ASSET_BUNDLE_FILE_EXT;

                if (BuildPipeline.BuildAssetBundle(obj, null, pathName, BuildAssetBundleOptions.CollectDependencies, targetPlatform))
                {
                    Debug.Log("The assetbundle file generated: " + pathName);
                }
            }
        }
        else
        {
            //pack all assets into one assetbundle file
            pathName = CommonPath.STREAMING_ASSETS + "All" + ASSET_BUNDLE_FILE_EXT;
            if (BuildPipeline.BuildAssetBundle(null, selectedAssets, pathName, BuildAssetBundleOptions.CollectDependencies, targetPlatform))
            {
                Debug.Log("The assetbundle file generated: " + pathName);
            }
        }

        //refresh asset database
        AssetDatabase.Refresh();
    }
}