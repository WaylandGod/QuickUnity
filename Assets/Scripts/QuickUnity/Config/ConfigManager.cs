using System.Collections;
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
    }
}