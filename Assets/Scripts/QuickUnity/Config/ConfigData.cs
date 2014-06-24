using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickUnity.Config
{
    /// <summary>
    /// Config data base class.
    /// </summary>
    public class ConfigData
    {
        /// <summary>
        /// A dictionary to hold key value pair of config data.
        /// </summary>
        private Dictionary<string, string> kvps;

        /// <summary>
        /// Parse the data from config file.
        /// </summary>
        /// <param name="kvps">A dictionary to hold key value pair of config data.</param>
        public virtual void ParseData(Dictionary<string, string> kvps)
        {
            this.kvps = kvps;
        }
    }
}