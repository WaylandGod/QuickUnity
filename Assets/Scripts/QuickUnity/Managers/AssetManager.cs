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

    private void Awake()
    {
        instance = this;
    }
}