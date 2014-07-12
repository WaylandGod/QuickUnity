using QuickUnity.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example to show how to use ConfigManager.
/// </summary>
[AddComponentMenu("")]
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

        // Test GetConfigData<T, int id>() method of ConfigManager
        Example data = configManager.GetConfigData<Example>(1);
        Debug.Log("id: " + data.id);

        // Test GetConfigData(System.Type, int id) method of ConfigManager
        data = (Example)configManager.GetConfigData(typeof(Example), 1);
        Debug.Log("boolTest:" + data.boolTest);

        //Test GetConfigDataDictionary<T>() of ConfigManager
        Dictionary<int, ConfigData> dictionary = configManager.GetConfigDataDictionary<Example>();
        data = (Example)dictionary[1];
        Debug.Log("level:" + data.level);

        //Test GetConfigDataDictionary(System.Type Type) method of ConfigManager
        dictionary = configManager.GetConfigDataDictionary(typeof(Example));
        data = (Example)dictionary[2];
        Debug.Log("name: " + data.name);
    }
}