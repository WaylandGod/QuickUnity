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
        /// Returns a value indicating the sign of floating-point number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Single.</returns>
        public static float Sign(float value)
        {
            if (value > 0.0f)
                return 1.0f;

            if (value < 0.0f)
                return -1.0f;

            return 0.0f;
        }

        /// <summary>
        /// Gets the reciprocal of a number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.Single.</returns>
        public static float GetReciprocal(float number)
        {
            return 1.0f / number;
        }

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