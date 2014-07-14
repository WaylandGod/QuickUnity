using QuickUnity.Utilitys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// The Config namespace.
/// </summary>
namespace QuickUnity.Config
{
    /// <summary>
    /// A class to mange everything about config waterHeightData.
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        /// The synchronize root.
        /// </summary>
        private static readonly object syncRoot = new object();

        /// <summary>
        /// The instance of singleton.
        /// </summary>
        private static ConfigManager instance;

        /// <summary>
        /// Gets the instance of singleton.
        /// </summary>
        /// <value>The instance of ConfigManager.</value>
        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConfigManager();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// The configuration waterHeightData table.
        /// </summary>
        private Dictionary<Type, Dictionary<int, ConfigData>> configDataDict;

        /// <summary>
        /// Prevents a default instance of the <see cref="ConfigManager"/> class from being created.
        /// </summary>
        private ConfigManager()
        {
            configDataDict = new Dictionary<Type, Dictionary<int, ConfigData>>();
        }

        /// <summary>
        /// Loads the configuration files.
        /// </summary>
        /// <param name="path">The path relative to the folder Resources.</param>
        public void LoadConfigFiles(string path)
        {
            TextAsset[] assets = Resources.LoadAll<TextAsset>(path);

            foreach (TextAsset asset in assets)
            {
                Dictionary<int, Dictionary<string, string>> voStringDict = ParseConfigData(asset.text);
                Dictionary<int, ConfigData> voDataDict = new Dictionary<int, ConfigData>();

                foreach (KeyValuePair<int, Dictionary<string, string>> kvp in voStringDict)
                {
                    ConfigData configData = (ConfigData)ReflectionUtility.CreateClassInstance(asset.name);
                    configData.ParseData(kvp.Value);
                    voDataDict.Add(kvp.Key, configData);
                }

                configDataDict.Add(Type.GetType(asset.name), voDataDict);
            }
        }

        /// <summary>
        /// Get the configuration waterHeightData dictionary.
        /// </summary>
        /// <typeparam name="T">The class of ConfigData.</typeparam>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.Int32,QuickUnity.Config.ConfigData&gt;.</returns>
        public Dictionary<int, ConfigData> GetConfigDataDictionary<T>() where T : ConfigData
        {
            Type type = typeof(T);
            return GetConfigDataDictionary(type);
        }

        /// <summary>
        /// Get the configuration waterHeightData dictionary.
        /// </summary>
        /// <param name="type">The type of ConfigData.</param>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.Int32,QuickUnity.Config.ConfigData&gt;.</returns>
        public Dictionary<int, ConfigData> GetConfigDataDictionary(Type type)
        {
            return configDataDict[type];
        }

        /// <summary>
        /// Get the configuration waterHeightData.
        /// </summary>
        /// <typeparam name="T">The class of ConfigData.</typeparam>
        /// <param name="id">The id of ConfigData.</param>
        /// <returns>QuickUnity.Config.ConfigData.</returns>
        public T GetConfigData<T>(int id) where T : ConfigData
        {
            Type type = typeof(T);
            return (T)GetConfigData(type, id);
        }

        /// <summary>
        /// Get the configuration waterHeightData.
        /// </summary>
        /// <param name="type">The type of ConfigData.</param>
        /// <param name="id">The id of ConfigData.</param>
        /// <returns>QuickUnity.Config.ConfigData.</returns>
        public ConfigData GetConfigData(Type type, int id)
        {
            if (configDataDict.ContainsKey(type))
            {
                Dictionary<int, ConfigData> voDataDict = configDataDict[type];
                return voDataDict[id];
            }

            return null;
        }

        /// <summary>
        /// Parses the configuration waterHeightData.
        /// </summary>
        /// <param name="text">The text content of configfuration waterHeightData.</param>
        /// <returns>Dictionary&lt;System.String, Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private Dictionary<int, Dictionary<string, string>> ParseConfigData(string text)
        {
            Dictionary<int, Dictionary<string, string>> voStringDict = new Dictionary<int, Dictionary<string, string>>();

            string[] textLines = text.Trim().Split("\r\n"[0]);
            string[] names = textLines[1].Split(","[0]);

            // Loop textLines except the first three lines.
            for (int i = 3, rows = textLines.Length; i < rows; ++i)
            {
                string textLine = textLines[i];
                string[] values = textLine.Split(","[0]);

                Dictionary<string, string> recordsets = new Dictionary<string, string>();

                // Loop cols.
                for (int j = 0, cols = names.Length; j < cols; ++j)
                {
                    string name = names[j];
                    string value = (j > values.Length - 1) ? "" : values[j];
                    recordsets.Add(name.Trim(), value.Trim());
                }

                string key = values[0];
                voStringDict.Add(int.Parse(key.Trim()), recordsets);
            }

            return voStringDict;
        }
    }
}