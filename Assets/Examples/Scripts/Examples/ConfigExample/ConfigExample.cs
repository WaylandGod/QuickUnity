using QuickUnity.Config;
using System.Collections;
using UnityEngine;

public class ConfigExample : MonoBehaviour
{
    /// <summary>
    /// The configuration manager
    /// </summary>
    private ConfigManager configManager;

    // Use this for initialization
    private void Start()
    {
        configManager = ConfigManager.Instance;
        configManager.LoadConfigFiles("Config");
    }
}