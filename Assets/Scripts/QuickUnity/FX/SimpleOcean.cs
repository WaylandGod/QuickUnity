using System.Collections;
using System.Collections.Generic;
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
        /// The count of tiles.
        /// </summary>
        public int tilesCount = 2;

        /// <summary>
        /// The normal map width.
        /// </summary>
        public int normalMapWidth = 128;

        /// <summary>
        /// The normal map height.
        /// </summary>
        public int normalMapHeight = 128;

        /// <summary>
        /// The RenderTexture width.
        /// </summary>
        public int renderTextureWidth = 128;

        /// <summary>
        /// The RenderTexture height.
        /// </summary>
        public int renderTextureHeight = 128;

        /// <summary>
        /// The width of tile polygon.
        /// </summary>
        public int tilePolygonWidth = 32;

        /// <summary>
        /// The height of tile polygon.
        /// </summary>
        public int tilePolygonHeight = 32;

        /// <summary>
        /// The ocean tile size.
        /// </summary>
        public Vector3 oceanTileSize;

        /// <summary>
        /// The material of simple ocean.
        /// </summary>
        public Material oceanMaterial;

        /// <summary>
        /// The enabled of reflection.
        /// </summary>
        public bool reflectionEnabled;

        /// <summary>
        /// The x offset of wind.
        /// </summary>
        private float windX = 10.0f;

        /// <summary>
        /// Gets the x offset of wind.
        /// </summary>
        /// <value>The x offset of wind.</value>
        public float WindX
        {
            get { return windX; }
            set
            {
                if (windX != value)
                {
                    windX = value;
                }
            }
        }

        /// <summary>
        /// The shader of simple ocean.
        /// </summary>
        private Shader oceanShader;

        /// <summary>
        /// The inverter of ocean tile size.
        /// </summary>
        private Vector2 oceanTileSizeInv;

        /// <summary>
        /// The reflection texture.
        /// </summary>
        private RenderTexture reflectionTexture;

        /// <summary>
        /// The refraction texture.
        /// </summary>
        private RenderTexture refractionTexture;

        /// <summary>
        /// The data of mesh surface.
        /// </summary>
        private ComplexF[] data;

        /// <summary>
        /// The tangent of x.
        /// </summary>
        private ComplexF[] tangentX;

        /// <summary>
        /// The vertex offset of wave spectra.
        /// </summary>
        private ComplexF[] vertexSpectra;

        /// <summary>
        /// The normal map of wave spectra.
        /// </summary>
        private ComplexF[] normalMapSpectra;

        /// <summary>
        /// The geometry width.
        /// </summary>
        private int geometryWidth;

        /// <summary>
        /// The geometry height.
        /// </summary>
        private int geometryHeight;

        /// <summary>
        /// The tiles LOD.
        /// </summary>
        private List<List<Mesh>> tilesLOD;

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        private void Start()
        {
            // Get the shader from material.
            if (oceanShader != null)
                oceanShader = oceanMaterial.shader;

            // Initialize oceanTileSizeInv when script start.
            oceanTileSizeInv = new Vector2(1.0f / oceanTileSize.x, 1.0f / oceanTileSize.z);

            SetupOffscreenRendering();

            data = new ComplexF[tilePolygonWidth * tilePolygonHeight];
            tangentX = new ComplexF[tilePolygonWidth * tilePolygonHeight];
            geometryWidth = tilePolygonWidth + 1;
            geometryHeight = tilePolygonHeight + 1;

            // Initialize tiles LOD list.
            tilesLOD = new List<List<Mesh>>();

            for (int i = 0, count = tilesCount * tilesCount; i < count; ++i)
            {
                tilesLOD.Add(new List<Mesh>());
            }

            // Initialize ocean tiles.
            for (int y = 0; y < tilesCount; ++y)
            {
                for (int x = 0; x < tilesCount; ++x)
                {
                    GameObject tile = new GameObject("Ocean Tile");

                    tile.transform.position = new Vector3(
                        (x - Mathf.Floor(tilesCount * 0.5f)) * oceanTileSize.x,
                        transform.position.y,
                        (y - Mathf.Floor(tilesCount * 0.5f)) * oceanTileSize.z
                        );

                    MeshFilter meshFilter = tile.AddComponent<MeshFilter>();
                    tile.AddComponent<MeshRenderer>();
                    tile.renderer.material = oceanMaterial;
                    tile.transform.parent = transform;

                    // Set layer to the parent layer.
                    tile.layer = gameObject.layer;
                    tilesLOD[0].Add(meshFilter.mesh);
                }
            }

            // Initialize wave spectra. v0 for vertex offset, n0 for normal map.
            vertexSpectra = new ComplexF[tilePolygonWidth * tilePolygonHeight];
            normalMapSpectra = new ComplexF[normalMapWidth * normalMapHeight];
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
        }

        /// <summary>
        /// Setup settings of offscreen rendering.
        /// </summary>
        private void SetupOffscreenRendering()
        {
            // if renderer reflection and refraction textures.
            if (reflectionEnabled)
            {
                reflectionTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, 0);
                refractionTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, 0);

                reflectionTexture.wrapMode = TextureWrapMode.Clamp;
                refractionTexture.wrapMode = TextureWrapMode.Clamp;

                reflectionTexture.isPowerOfTwo = true;
                refractionTexture.isPowerOfTwo = true;

                oceanMaterial.SetTexture("_Reflection", reflectionTexture);
                oceanMaterial.SetTexture("_Refraction", refractionTexture);
                oceanMaterial.SetVector("_MaterialSize", new Vector4(oceanTileSize.x, oceanTileSize.y, oceanTileSize.z, 0.0f));
            }

            // Add MeshRenderer component.
            gameObject.AddComponent<MeshRenderer>();

            // Setup renderer.
            renderer.material.renderQueue = 1001;
            renderer.receiveShadows = false;
            renderer.castShadows = false;

            // Setup ocean surface mesh.
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            float minSizeX = -1024.0f;
            float maxSizeX = 1024.0f;

            float minSizeY = -1024.0f;
            float maxSizeY = 1024.0f;

            //The coordinates of vertices.
            vertices[0] = new Vector3(minSizeX, 0.0f, maxSizeY);
            vertices[1] = new Vector3(maxSizeX, 0.0f, maxSizeY);
            vertices[2] = new Vector3(maxSizeX, 0.0f, minSizeY);
            vertices[3] = new Vector3(minSizeX, 0.0f, minSizeY);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 0;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.normals = new Vector3[4];
            mesh.triangles = triangles;

            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

            if (meshFilter == null)
                meshFilter = gameObject.AddComponent<MeshFilter>();

            meshFilter.mesh = mesh;
            mesh.RecalculateBounds();

            // Prevent bounds will be calculated automatically.
            vertices[0] = Vector3.zero;
            vertices[1] = Vector3.zero;
            vertices[2] = Vector3.zero;
            vertices[3] = Vector3.zero;

            mesh.vertices = vertices;
        }
    }
}