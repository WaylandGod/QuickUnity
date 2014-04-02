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
    private const string ASSET_BUNDLE_FILE_EXT = ".unity3d";

    [MenuItem("QuickUnity/Assets/Build AssetBundles")]
    public static void BuildAssetBundlesForWin()
    {
        BuildAssetBundles();
    }

    [MenuItem("QuickUnity/Assets/Build AssetBundle All In One")]
    public static void BuildAssetBundleAllInOneForWin()
    {
        BuildAssetBundles(false);
    }

    private static void BuildAssetBundles(bool isStandalone = true, BuildTarget targetPlatform = BuildTarget.StandaloneWindows)
    {
        string path = EditorUtility.SaveFolderPanel("Save AssetBundles", "", "");

        Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string pathName = "";

        if (path == "")
            return;

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
                pathName = path + "/" + obj.name + ASSET_BUNDLE_FILE_EXT;

                if (BuildPipeline.BuildAssetBundle(obj, null, pathName, BuildAssetBundleOptions.CollectDependencies, targetPlatform))
                {
                    Debug.Log("The assetbundle file generated: " + pathName);
                }
            }
        }
        else
        {
            //pack all assets into one assetbundle file
            pathName = path + "/All" + ASSET_BUNDLE_FILE_EXT;
            if (BuildPipeline.BuildAssetBundle(null, selectedAssets, pathName, BuildAssetBundleOptions.CollectDependencies, targetPlatform))
            {
                Debug.Log("The assetbundle file generated: " + pathName);
            }
        }

        //refresh asset database
        AssetDatabase.Refresh();
    }
}