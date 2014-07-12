using UnityEngine;

/// <summary>
/// The Utilitys namespace.
/// </summary>
namespace QuickUnity.Utilitys
{
    /// <summary>
    /// A utility class for doing math calculation. This class cannot be inherited.
    /// </summary>
    public sealed class MathUtility
    {
        /// <summary>
        /// Generate gaussian random number.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GenGaussianRnd()
        {
            float x1 = Random.value;
            float x2 = Random.value;

            if (x1 == 0.0f)
                x1 = 0.01f;

            return (float)(System.Math.Sqrt(-2.0 * System.Math.Log(x1)) * System.Math.Cos(2.0 * Mathf.PI * x2));
        }
    }
}