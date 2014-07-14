using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Config namespace.
/// </summary>
namespace QuickUnity.Config
{
    /// <summary>
    /// Config waterHeightData base class.
    /// </summary>
    public class ConfigData
    {
        /// <summary>
        /// The error message of none attribute.
        /// </summary>
        private const string ERROR_KEY_NOT_FOUND = " can not be found !";

        /// <summary>
        /// A dictionary to hold key value pair of config waterHeightData.
        /// </summary>
        private Dictionary<string, string> kvps;

        /// <summary>
        /// Parse the waterHeightData from config file.
        /// </summary>
        /// <param name="kvps">A dictionary to hold key value pair of config waterHeightData.</param>
        public virtual void ParseData(Dictionary<string, string> kvps)
        {
            this.kvps = kvps;
        }

        /// <summary>
        /// Reads the bool value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns><c>true</c> if value is 'TRUE', <c>false</c> otherwise.</returns>
        protected bool ReadBool(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];

            if (value == "TRUE")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Reads the byte value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.Byte.</returns>
        protected byte ReadByte(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return byte.Parse(value);
        }

        /// <summary>
        /// Reads the double value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.Double.</returns>
        protected double ReadDouble(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return double.Parse(value);
        }

        /// <summary>
        /// Reads the float value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.Single.</returns>
        protected float ReadFloat(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return float.Parse(value);
        }

        /// <summary>
        /// Reads the int value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.Int32.</returns>
        protected int ReadInt(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return int.Parse(value);
        }

        /// <summary>
        /// Reads the long value-.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.Int64.</returns>
        protected long ReadLong(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return long.Parse(value);
        }

        /// <summary>
        /// Reads the short value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.Int16.</returns>
        protected short ReadShort(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return short.Parse(value);
        }

        /// <summary>
        /// Reads the string value.
        /// </summary>
        /// <param name="key">The key of key value pair.</param>
        /// <returns>System.String.</returns>
        protected string ReadString(string key)
        {
            if (!kvps.ContainsKey(key))
                Debug.LogError(key + ERROR_KEY_NOT_FOUND);

            string value = kvps[key];
            return value;
        }
    }
}