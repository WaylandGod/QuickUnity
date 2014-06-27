using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace QuickUnity.Config
{
    /// <summary>
    /// A class to mange everything about config data.
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
        /// Prevents a default instance of the <see cref="ConfigManager"/> class from being created.
        /// </summary>
        private ConfigManager()
        {
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
                //string className = Path.GetFileNameWithoutExtension(AssetDatabase.asset);
                //Type classType = Type.GetType(className);
                //Assembly assembly = classType.Assembly;
                ParseConfigData(asset.text);
            }
        }

        /// <summary>
        /// Parses the configuration data.
        /// </summary>
        /// <param name="text">The text content of configfuration data.</param>
        /// <returns>Dictionary&lt;System.String, Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private Dictionary<string, Dictionary<string, string>> ParseConfigData(string text)
        {
            Dictionary<string, Dictionary<string, string>> dataTable = new Dictionary<string, Dictionary<string, string>>();

            string[] textLines = text.Trim().Split("\r\n"[0]);
            string[] names = textLines[1].Split(","[0]);

            // Loop textLines except the first three lines.
            for (int i = 3, rows = textLines.Length; i < rows; ++i)
            {
                string textLine = textLines[i];
                string[] values = textLine.Split(","[0]);

                Dictionary<string, string> recordset = new Dictionary<string, string>();

                // Loop cols.
                for (int j = 0, cols = names.Length; j < cols; ++j)
                {
                    string name = names[j];
                    string value = values[j];
                    recordset.Add(name, value);
                }

                string key = values[0];
                dataTable.Add(key, recordset);
            }

            return dataTable;
        }
    }
}