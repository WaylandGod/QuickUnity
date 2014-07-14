using UnityEngine;

/// <summary>
/// The Utilitys namespace.
/// </summary>
namespace QuickUnity.Utilitys
{
    /// <summary>
    /// A utility class to help you process object Vector2, Vector3 and Vector4.
    /// </summary>
    public sealed class VectorUtility
    {
        /// <summary>
        /// Gets the reciprocal of a Vector2 object.
        /// </summary>
        /// <param name="vector">The vector of Vector2.</param>
        /// <returns>Vector2.</returns>
        public static Vector2 GetVector2Reciprocal(Vector2 vector)
        {
            return new Vector2(1.0f / vector.x, 1.0f / vector.y);
        }
    }
}