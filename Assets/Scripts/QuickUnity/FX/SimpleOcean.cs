using QuickUnity.Utilitys;
using System;
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
    [AddComponentMenu("QuickUnity/FX/Simple Ocean")]
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
        /// The surface color of ocean.
        /// </summary>
        private Color surfaceColor = Color.white;

        /// <summary>
        /// Gets or sets the color of the surface.
        /// </summary>
        /// <value>The color of the surface.</value>
        public Color SurfaceColor
        {
            get { return surfaceColor; }
            set
            {
                if (surfaceColor != value)
                {
                    surfaceColor = value;
                    UpdateWaterColor();
                }
            }
        }

        /// <summary>
        /// The water color of ocean.
        /// </summary>
        private Color waterColor = Color.blue;

        /// <summary>
        /// Gets or sets the color of the water.
        /// </summary>
        /// <value>The color of the water.</value>
        public Color WaterColor
        {
            get { return waterColor; }
            set
            {
                if (waterColor != value)
                {
                    waterColor = value;
                    UpdateWaterColor();
                }
            }
        }

        /// <summary>
        /// The normal map scale.
        /// </summary>
        private int normalMapScale = 8;

        /// <summary>
        /// Gets or sets the normal map scale.
        /// </summary>
        /// <value>The normal map scale.</value>
        public int NormalMapScale
        {
            get { return normalMapScale; }
            set
            {
                if (normalMapScale != value)
                {
                    normalMapScale = value;
                    InitializeWaveGenerator();
                }
            }
        }

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
                    InitializeWaveGenerator();
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
        /// The base mesh.
        /// </summary>
        private Mesh baseMesh;

        /// <summary>
        /// The tiles LOD.
        /// </summary>
        private List<List<Mesh>> tilesLOD;

        /// <summary>
        /// Gets the maximum LOD of tiles.
        /// </summary>
        /// <value>The maximum LOD.</value>
        private int MaxLOD
        {
            get { return tilesCount * tilesCount; }
        }

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

            for (int i = 0, count = MaxLOD; i < count; ++i)
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

            InitializeWaveGenerator();
            UpdateWaterColor();
            GenHeightmap();
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

        /// <summary>
        /// Initializes the wave generator.
        /// </summary>
        private void InitializeWaveGenerator()
        {
            // Wind was restricted to one direction, reduces calculations.
            Vector2 wind = new Vector2(windX, 0.0f);

            for (int y = 0; y < tilePolygonHeight; ++y)
            {
                for (int x = 0; x < tilePolygonWidth; ++x)
                {
                    float yCopy = y < tilePolygonHeight / 2.0f ? y : y - tilePolygonHeight;
                    float xCopy = x < tilePolygonWidth / 2.0f ? x : x - tilePolygonWidth;
                    Vector2 vecK = new Vector2(2.0f * Mathf.PI * xCopy / oceanTileSize.x, 2.0f * Mathf.PI * yCopy / oceanTileSize.z);
                    vertexSpectra[tilePolygonWidth * y + x] = new ComplexF(MathUtility.GenGaussianRnd(), MathUtility.GenGaussianRnd()) * 0.707f * (float)Math.Sqrt(PhillipsSpectrum(vecK, wind));
                }
            }

            for (int y = 0; y < renderTextureHeight; ++y)
            {
                for (int x = 0; x < renderTextureWidth; ++x)
                {
                    float yCopy = y < renderTextureHeight / 2.0f ? y : y - renderTextureHeight;
                    float xCopy = x < renderTextureWidth / 2.0f ? x : x - renderTextureWidth;
                    Vector2 vecK = new Vector2(2.0f * Mathf.PI * xCopy / (oceanTileSize.x / normalMapScale), 2.0f * Mathf.PI * yCopy / (oceanTileSize.z / normalMapScale));
                    normalMapSpectra[renderTextureWidth * y + x] = new ComplexF(MathUtility.GenGaussianRnd(), MathUtility.GenGaussianRnd() * 0.707f * (float)Math.Sqrt(PhillipsSpectrum(vecK, wind)));
                }
            }
        }

        /// <summary>
        /// Updates the color of the water.
        /// </summary>
        private void UpdateWaterColor()
        {
            oceanMaterial.SetColor("_SurfaceColor", surfaceColor);
            oceanMaterial.SetColor("_WaterColor", waterColor);
        }

        /// <summary>
        /// Generate the heightmap.
        /// </summary>
        private void GenHeightmap()
        {
            Mesh mesh = new Mesh();

            int y = 0;
            int x = 0;

            // Build vertices and uv.
            Vector3[] vertices = new Vector3[geometryWidth * geometryHeight];
            Vector4[] tangents = new Vector4[geometryWidth * geometryHeight];
            Vector2[] uv = new Vector2[geometryWidth * geometryHeight];

            Vector2 uvScale = new Vector2(1.0f / (geometryWidth - 1.0f), 1.0f / (geometryHeight - 1.0f));
            Vector3 sizeScale = new Vector3(oceanTileSize.x / (geometryWidth - 1.0f), oceanTileSize.y, oceanTileSize.z / (geometryHeight - 1.0f));

            for (y = 0; y < geometryHeight; ++y)
            {
                for (x = 0; x < geometryWidth; ++x)
                {
                    Vector3 vertex = new Vector3(x, 0.0f, y);
                    vertices[y * geometryWidth + x] = Vector3.Scale(vertex, sizeScale);
                    uv[y * geometryWidth + x] = Vector2.Scale(new Vector2(x, y), uvScale);
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;

            // Build tangents.
            for (y = 0; y < geometryHeight; ++y)
            {
                for (x = 0; x < geometryWidth; ++x)
                {
                    tangents[y * geometryWidth + x] = new Vector4(1.0f, 0.0f, 0.0f, -1.0f);
                }
            }

            mesh.tangents = tangents;

            int i = 0;
            int j = 0;
            int count = MaxLOD;

            for (i = 0; i < count; ++i)
            {
                int length = (int)(tilePolygonHeight / Math.Pow(2, i) + 1) * (int)(tilePolygonWidth / Math.Pow(2, i) + 1);
                Vector3[] verticesLOD = new Vector3[length];
                Vector2[] uvLOD = new Vector2[length];

                int idx = 0;
                int pow = (int)Math.Pow(2, i);

                for (y = 0; y < geometryHeight; y += pow)
                {
                    for (x = 0; x < geometryWidth; x += pow)
                    {
                        verticesLOD[idx] = vertices[geometryWidth * y + x];
                        uvLOD[idx++] = uv[geometryWidth * y + x];
                    }
                }

                int tilesLODCount = tilesLOD[i].Count;

                for (j = 0; j < tilesLODCount; ++j)
                {
                    Mesh meshLOD = tilesLOD[i][j];
                    meshLOD.vertices = verticesLOD;
                    meshLOD.uv = uvLOD;
                }
            }

            // Build triangle indices: 3 indices into vertex array for each triangle.
            for (i = 0; i < count; ++i)
            {
                int idx = 0;
                int widthLOD = (int)(tilePolygonWidth / Math.Pow(2, i) + 1);
                int[] triangles = new int[(int)(tilePolygonHeight / Math.Pow(2, i) * tilePolygonWidth / Math.Pow(2, i)) * 6];

                int height = (int)(tilePolygonHeight / Math.Pow(2, i));
                int width = (int)(tilePolygonWidth / Math.Pow(2, i));

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        // For each grid cell output two triangle.
                        triangles[idx++] = y * widthLOD + x;
                        triangles[idx++] = (y + 1) * widthLOD + x;
                        triangles[idx++] = y * widthLOD + x + 1;

                        triangles[idx++] = (y + 1) * widthLOD + x;
                        triangles[idx++] = (y + 1) * widthLOD + x + 1;
                        triangles[idx++] = y * widthLOD + x + 1;
                    }
                }

                int tilesLODCount = tilesLOD[i].Count;

                for (j = 0; j < tilesLODCount; ++j)
                {
                    Mesh meshLOD = tilesLOD[i][j];
                    meshLOD.triangles = triangles;
                }
            }

            baseMesh = mesh;
        }

        /// <summary>
        /// Phillips Spectrum algorithm.
        /// </summary>
        /// <param name="vecK">The vector K.</param>
        /// <param name="wind">The vector wind.</param>
        /// <returns>System.Single.</returns>
        private float PhillipsSpectrum(Vector2 vecK, Vector2 wind)
        {
            float A = vecK.x > 0.0f ? 1.0f : 0.05f; // Set wind to blow only in one direction - otherwise we get turmoiling water.

            float L = wind.sqrMagnitude / 9.81f;
            float k2 = vecK.sqrMagnitude;

            // Avoid division by zero
            if (k2 == 0.0f)
                return k2;

            float vecKMagnitude = vecK.magnitude;
            return (float)(A * Math.Exp(-1.0f / (k2 * L * L) - Math.Pow(vecKMagnitude * 0.1, 2.0f)) / (k2 * k2) * Math.Pow(Vector2.Dot(vecK / vecKMagnitude, wind / wind.magnitude), 2.0f));
        }
    }
}