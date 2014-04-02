/*************************************************************************************
     * Class Name:  AssetManager
     * Namespace:
     * Author：       cosmos53076@163.com
     * Description:  A manager class to handle something about assets.
    *************************************************************************************/

using System.Collections;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static AssetManager instance;

    public static AssetManager GetInstance()
    {
        return instance;
    }

    public delegate void EventHandler(Asset asset);

    public event EventHandler OnProgress;

    public event EventHandler OnComplete;

    private void Awake()
    {
        instance = this;
    }
}

public class Asset
{
    private float mProgress = 0.0f;

    public float progess
    {
        get
        {
            return mProgress;
        }
    }

    public Asset(float progress = 0.0f)
    {
        mProgress = progress;
    }
}