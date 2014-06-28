using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace QuickUnity.Utilitys
{
    /// <summary>
    /// The utility class about reflection functions. This class cannot be inherited.
    /// </summary>
    public sealed class ReflectionUtility
    {
        /// <summary>
        /// Creates the class instance dynamically.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns>System.Object.</returns>
        public static object CreateClassInstance(string className)
        {
            Type classType = Type.GetType(className);
            Assembly assembly = classType.Assembly;
            return assembly.CreateInstance(className);
        }
    }
}