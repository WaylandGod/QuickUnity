using System.Collections;
using UnityEngine;

/// <summary>
/// The FX namespace.
/// </summary>
namespace QuickUnity.FX
{
    /// <summary>
    /// Simple ocean visual effect.
    /// </summary>
    public class SimpleOcean : MonoBehaviour
    {
        /// <summary>
        /// The normal map width.
        /// </summary>
        public int normalMapWidth = 128;

        /// <summary>
        /// The normal map height.
        /// </summary>
        public int normalMapHeight = 128;

        /// <summary>
        /// The ocean size.
        /// </summary>
        public Vector3 oceanSize;

        /// <summary>
        /// The material of simple ocean.
        /// </summary>
        public Material oceanMaterial;

        /// <summary>
        /// The shader of simple ocean.
        /// </summary>
        private Shader oceanShader;

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        private void Start()
        {
            // Get the shader from material.
            if (oceanShader != null)
                oceanShader = oceanMaterial.shader;
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
        }
    }
}