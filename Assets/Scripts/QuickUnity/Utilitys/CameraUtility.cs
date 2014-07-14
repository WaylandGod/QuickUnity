using UnityEngine;

/// <summary>
/// The Utilitys namespace.
/// </summary>
namespace QuickUnity.Utilitys
{
    /// <summary>
    /// A utility class to help you process Camera object.
    /// </summary>
    public sealed class CameraUtility
    {
        /// <summary>
        /// Calculates the oblique matrix.
        /// </summary>
        /// <param name="projection">The projection.</param>
        /// <param name="clipPane">The clip pane.</param>
        public static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPane)
        {
            Vector4 a = projection.inverse * new Vector4(
                MathUtility.Sign(clipPane.x),
                MathUtility.Sign(clipPane.y),
                1.0f,
                1.0f
                );

            Vector4 b = clipPane * (2.0f / (Vector4.Dot(clipPane, a)));
            projection[2] = b.x - projection[3];
            projection[6] = b.y - projection[7];
            projection[10] = b.z - projection[11];
            projection[14] = b.w - projection[15];
        }

        /// <summary>
        /// Get the space clip plane of camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="position">The position.</param>
        /// <param name="normal">The normal.</param>
        /// <param name="sideSign">The side sign.</param>
        /// <returns>Vector4.</returns>
        public static Vector4 CameraSpaceClipPlane(Camera camera, Vector3 position, Vector3 normal, float sideSign)
        {
            Vector3 offsetPosition = position + normal * 0.02f;
            Matrix4x4 matrix = camera.worldToCameraMatrix;
            Vector3 cameraPosition = matrix.MultiplyPoint(offsetPosition);
            Vector3 cameraNormal = matrix.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(cameraNormal.x, cameraNormal.y, cameraNormal.z, -Vector3.Dot(cameraPosition, cameraNormal));
        }

        /// <summary>
        /// Calculates the reflection matrix.
        /// </summary>
        /// <param name="reflectionMatrix">The reflection matrix.</param>
        /// <param name="plane">The plane.</param>
        public static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMatrix, Vector4 plane)
        {
            reflectionMatrix.m00 = 1.0f - 2.0f * plane.x * plane.x;
            reflectionMatrix.m01 = -2.0f * plane.x * plane.y;
            reflectionMatrix.m02 = -2.0f * plane.x * plane.z;
            reflectionMatrix.m03 = -2.0f * plane.w * plane.x;

            reflectionMatrix.m10 = -2.0f * plane.y * plane.x;
            reflectionMatrix.m11 = 1.0f - 2.0f * plane.y * plane.y;
            reflectionMatrix.m12 = -2.0f * plane.y * plane.z;
            reflectionMatrix.m13 = -2.0f * plane.z * plane.y;

            reflectionMatrix.m20 = -2.0f * plane.z * plane.x;
            reflectionMatrix.m21 = -2.0f * plane.z * plane.y;
            reflectionMatrix.m22 = 1.0f - 2.0f * plane.z * plane.z;
            reflectionMatrix.m23 = -2.0f * plane.w * plane.z;

            reflectionMatrix.m30 = 0.0f;
            reflectionMatrix.m31 = 0.0f;
            reflectionMatrix.m32 = 0.0f;
            reflectionMatrix.m33 = 1.0f;
        }
    }
}