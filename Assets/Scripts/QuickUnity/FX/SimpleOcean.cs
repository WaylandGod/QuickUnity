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
        /// The gravity acceleration constant.
        /// </summary>
        private const float GRAVITY_ACCELERATION = 9.81f;

        /// <summary>
        /// The humidity of force storm.
        /// </summary>
        private const float FORCE_STORM_HUMIDITY = 1.0f;

        /// <summary>
        /// The humidit update frequency.
        /// </summary>
        private const float HUMIDITY_UPDATE_FREQUENCY = 1.0f / 280.0f;

        /// <summary>
        /// The maximum LOD.
        /// </summary>
        private const int MAX_LOD = 4;

        /// <summary>
        /// The render layers.
        /// </summary>
        public LayerMask renderLayers = -1;

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
        /// The wave speed.
        /// </summary>
        public float waveSpeed = 0.7f;

        /// <summary>
        /// The wave scale.
        /// </summary>
        public float waveScale = 0.1f;

        /// <summary>
        /// The choppy scale.
        /// </summary>
        public float choppyScale = 2.0f;

        /// <summary>
        /// The light direction.
        /// </summary>
        [SerializeField]
        private Vector4 lightDirection;

        /// <summary>
        /// Gets or sets the light direction.
        /// </summary>
        /// <value>The light direction.</value>
        public Vector4 LightDirection
        {
            get { return lightDirection; }
            set
            {
                if (lightDirection != value)
                {
                    lightDirection = value;
                    UpdateLightDirection();
                }
            }
        }

        /// <summary>
        /// The force storm.
        /// </summary>
        public bool forceStorm;

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
        [SerializeField]
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
        [SerializeField]
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
        /// The reciprocal of ocean tile size.
        /// </summary>
        private Vector2 oceanTileSizeReciprocal;

        /// <summary>
        /// The reflection texture.
        /// </summary>
        private RenderTexture reflectionTexture;

        /// <summary>
        /// The refraction texture.
        /// </summary>
        private RenderTexture refractionTexture;

        /// <summary>
        /// The height data of mesh surface.
        /// </summary>
        private ComplexF[] waterHeightData;

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
        /// The base value for vertices and uv coordinates.
        /// </summary>
        private Vector3[] baseHeights;

        /// <summary>
        /// The vertices of mesh.
        /// </summary>
        private Vector3[] vertices;

        /// <summary>
        /// The normals of mesh.
        /// </summary>
        private Vector3[] normals;

        /// <summary>
        /// The tangents of mesh.
        /// </summary>
        private Vector4[] tangents;

        /// <summary>
        /// The tiles LOD.
        /// </summary>
        private List<List<Mesh>> tilesLOD;

        /// <summary>
        /// The offscreen camera for rendering reflection and refraction.
        /// </summary>
        private Camera offscreenCamera;

        /// <summary>
        /// The Transform object of offscreen camera.
        /// </summary>
        private Transform offscreenCameraTransform;

        /// <summary>
        /// The Transform object of main camera.
        /// </summary>
        private Transform mainCameraTransform;

        /// <summary>
        /// The humidity parameter.
        /// </summary>
        private float humidity = 0.1f;

        /// <summary>
        /// The previous humidity value.
        /// </summary>
        private float prevHumidityValue = 0.1f;

        /// <summary>
        /// The next humidity value.
        /// </summary>
        private float nextHumidityValue = 0.4f;

        /// <summary>
        /// The previous humidity update time.
        /// </summary>
        private float prevHumidityUpdateTime;

        /// <summary>
        /// The wave scale in real time.
        /// </summary>
        private float waveScaleRealTime;

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        private void Start()
        {
            // Initialize oceanTileSizeReciprocal when script start.
            oceanTileSizeReciprocal = VectorUtility.GetVector2Reciprocal(oceanTileSize);

            SetupOffscreenRendering();

            waterHeightData = new ComplexF[tilePolygonWidth * tilePolygonHeight];
            tangentX = new ComplexF[tilePolygonWidth * tilePolygonHeight];
            geometryWidth = tilePolygonWidth + 1;
            geometryHeight = tilePolygonHeight + 1;

            // Initialize tiles LOD list.
            tilesLOD = new List<List<Mesh>>();

            for (int i = 0, count = MAX_LOD; i < count; ++i)
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
                        transform.position.x + (x - Mathf.Floor(tilesCount * 0.5f)) * oceanTileSize.x,
                        transform.position.y,
                        transform.position.z + (y - Mathf.Floor(tilesCount * 0.5f)) * oceanTileSize.z
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

            if (Camera.main != null)
                mainCameraTransform = Camera.main.transform;

            //Update Wave
            StartCoroutine(UpdateWave());
        }

        /// <summary>
        /// Called when activeSlef is false.
        /// </summary>
        private void OnDisable()
        {
            DestroyReflectionTexture();
            DestroyRefractionTexture();
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            // Calculate mesh vertices, uv and tangents.
            float halfWidth = tilePolygonWidth / 2.0f;
            float halfHeight = tilePolygonHeight / 2.0f;
            float time = Time.time;

            for (int y = 0; y < tilePolygonHeight; ++y)
            {
                for (int x = 0; x < tilePolygonWidth; ++x)
                {
                    int idx = tilePolygonWidth * y + x;
                    float yCopy = y < halfHeight ? y : y - tilePolygonHeight;
                    float xCopy = x < halfWidth ? x : x - tilePolygonWidth;
                    Vector2 vecK = new Vector2(2.0f * Mathf.PI * xCopy / oceanTileSize.x, 2.0f * Mathf.PI * yCopy / oceanTileSize.z);

                    float sqrtMagnitude = (float)Math.Sqrt(Mathf.Pow(vecK.x, 2.0f) + Mathf.Pow(vecK.y, 2.0f));
                    float offset = Mathf.Sqrt(GRAVITY_ACCELERATION * sqrtMagnitude) * time * waveSpeed;
                    ComplexF complexFA = new ComplexF(Mathf.Cos(offset), Mathf.Sin(offset));
                    ComplexF complexFB;
                    complexFB.Re = complexFA.Re;
                    complexFB.Im = -complexFA.Im;

                    int nY = y > 0 ? tilePolygonHeight - y : 0;
                    int nX = x > 0 ? tilePolygonWidth - x : 0;

                    waterHeightData[idx] = vertexSpectra[idx] * complexFA + vertexSpectra[tilePolygonWidth * nY + nX].GetConjugate() * complexFB;
                    tangentX[idx] = waterHeightData[idx] * new ComplexF(0.0f, vecK.x) - waterHeightData[idx] * vecK.y;

                    // Choppy wave calculation.
                    if (x + y > 0)
                        waterHeightData[idx] += waterHeightData[idx] * vecK.x / sqrtMagnitude;
                }
            }

            Fourier.FFT2(waterHeightData, tilePolygonWidth, tilePolygonHeight, FourierDirection.Backward);
            Fourier.FFT2(tangentX, tilePolygonWidth, tilePolygonHeight, FourierDirection.Backward);

            // Get base values for vertices and uv coordinates.
            if (baseHeights == null)
            {
                baseHeights = baseMesh.vertices;
                vertices = new Vector3[baseHeights.Length];
                normals = new Vector3[baseHeights.Length];
                tangents = new Vector4[baseHeights.Length];
            }

            int area = tilePolygonWidth * tilePolygonHeight;
            float scaleX = choppyScale / area;
            float scaleY = waveScaleRealTime / area;
            float scaleYReciprocal = MathUtility.GetReciprocal(scaleY);

            for (int i = 0; i < area; ++i)
            {
                int index = i + i / tilePolygonWidth;
                vertices[index] = baseHeights[index];
                vertices[index].x += waterHeightData[i].Im * scaleX;
                vertices[index].y = waterHeightData[i].Re * scaleY;

                normals[index] = Vector3.Normalize(new Vector3(tangentX[i].Re, scaleYReciprocal, tangentX[i].Im));

                if ((i + 1) % tilePolygonWidth == 0)
                {
                    int indexPlus = index + 1;
                    int iWidth = i + 1 - tilePolygonWidth;
                    vertices[indexPlus] = baseHeights[indexPlus];
                    vertices[indexPlus].x += waterHeightData[iWidth].Im * scaleX;
                    vertices[indexPlus].y = waterHeightData[iWidth].Re * scaleY;

                    normals[indexPlus] = Vector3.Normalize(new Vector3(tangentX[iWidth].Re, scaleYReciprocal, tangentX[iWidth].Im));
                }
            }

            int indexOffset = geometryWidth * (geometryHeight - 1);

            for (int i = 0; i < geometryWidth; ++i)
            {
                int index = i + indexOffset;
                int mod = i % tilePolygonWidth;

                vertices[index] = baseHeights[index];
                vertices[index].x += waterHeightData[mod].Im * scaleX;
                vertices[index].y = waterHeightData[mod].Re * scaleY;

                normals[index] = Vector3.Normalize(new Vector3(tangentX[mod].Re, scaleYReciprocal, tangentX[mod].Im));
            }

            int geometryArea = geometryWidth * geometryHeight - 1;

            for (int i = 0; i < geometryArea; ++i)
            {
                Vector3 tmp;

                if ((i + 1) % geometryWidth == 0)
                    tmp = Vector3.Normalize(vertices[i - tilePolygonWidth + 1] + new Vector3(oceanTileSize.x, 0.0f, 0.0f) - vertices[i]);
                else
                    tmp = Vector3.Normalize(vertices[i + 1] - vertices[i]);

                tangents[i] = new Vector4(tmp.x, tmp.y, tmp.z, tangents[i].w);
            }

            for (int y = 0; y < geometryHeight; ++y)
            {
                for (int x = 0; x < geometryWidth; ++x)
                {
                    int index = x + geometryWidth * y;

                    if (x + 1 >= geometryWidth)
                    {
                        tangents[index].w = tangents[geometryWidth * y].w;
                        continue;
                    }

                    if (y + 1 >= geometryHeight)
                    {
                        tangents[index].w = tangents[x].w;
                        continue;
                    }

                    float right = vertices[x + 1 + geometryWidth * y].x - vertices[index].x;
                    float foam = right / (oceanTileSize.x / geometryWidth);

                    if (foam < 0.0f)
                        tangents[index].w = 1.0f;
                    else if (foam < 0.5f)
                        tangents[index].w += 3.0f * Time.deltaTime;
                    else
                        tangents[index].w -= 0.4f * Time.deltaTime;

                    tangents[index].w = Mathf.Clamp(tangents[index].w, 0.0f, 2.0f);
                }
            }

            tangents[geometryArea] = Vector4.Normalize(vertices[geometryArea] + new Vector3(oceanTileSize.x, 0.0f, 0.0f) - vertices[1]);

            for (int level = 0; level < MAX_LOD; ++level)
            {
                int pow = (int)Math.Pow(2.0f, level);
                int length = (int)((tilePolygonHeight / pow + 1) * (tilePolygonWidth / pow + 1));

                Vector4[] tangentsLOD = new Vector4[length];
                Vector3[] verticesLOD = new Vector3[length];
                Vector3[] normalsLOD = new Vector3[length];

                int index = 0;

                for (int y = 0; y < geometryHeight; y += pow)
                {
                    for (int x = 0; x < geometryWidth; x += pow)
                    {
                        int indexTemp = geometryWidth * y + x;
                        verticesLOD[index] = vertices[indexTemp];
                        tangentsLOD[index] = tangents[indexTemp];
                        normalsLOD[index++] = normals[indexTemp];
                    }
                }

                for (int i = 0, count = tilesLOD[level].Count; i < count; ++i)
                {
                    Mesh meshLOD = tilesLOD[level][i];
                    meshLOD.vertices = verticesLOD;
                    meshLOD.normals = normalsLOD;
                    meshLOD.tangents = tangentsLOD;
                }
            }

            if (reflectionEnabled)
                RenderReflectionAndRefraction();
        }

        /// <summary>
        /// Creates the render textures of reflection and refraction.
        /// </summary>
        private void CreateRenderTextures()
        {
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
        }

        /// <summary>
        /// Setup settings of offscreen rendering.
        /// </summary>
        private void SetupOffscreenRendering()
        {
            if (oceanMaterial != null)
                oceanMaterial.SetVector("_LightDir", lightDirection);

            // if renderer reflection and refraction textures.
            CreateRenderTextures();

            // Create offscreeen camera to render reflection and refraction.
            GameObject cameraObj = new GameObject();
            cameraObj.name = "Offscreeen Camera";
            cameraObj.transform.parent = transform;

            offscreenCamera = cameraObj.AddComponent<Camera>();
            offscreenCamera.clearFlags = CameraClearFlags.Color;
            offscreenCamera.depth = -1;
            offscreenCamera.enabled = false;
            offscreenCameraTransform = offscreenCamera.transform;

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
        /// Renders the reflection and refraction of ocean.
        /// </summary>
        private void RenderReflectionAndRefraction()
        {
            if (Camera.current == offscreenCamera)
                return;

            if (reflectionTexture == null || refractionTexture == null)
                return;

            if (mainCameraTransform == null)
                mainCameraTransform = Camera.main.transform;

            Camera mainCamera = Camera.mainCamera;
            Matrix4x4 originalWorldToCamera = mainCamera.worldToCameraMatrix;
            int cullingMask = ~(1 << 4) & renderLayers.value;

            // Reflection pass.
            float y = -transform.position.y;
            Matrix4x4 reflectionMatrix = Matrix4x4.zero;
            CameraUtility.CalculateReflectionMatrix(ref reflectionMatrix, new Vector4(0.0f, 1.0f, 0.0f, y));

            offscreenCamera.backgroundColor = RenderSettings.fogColor;
            offscreenCameraTransform.position = reflectionMatrix.MultiplyPoint(mainCameraTransform.position);
            offscreenCameraTransform.rotation = mainCameraTransform.rotation;
            offscreenCamera.worldToCameraMatrix = originalWorldToCamera * reflectionMatrix;
            offscreenCamera.cullingMask = cullingMask;
            offscreenCamera.targetTexture = reflectionTexture;

            // Need to reverse face culling for reflection pass, since the camera is now flipped upside/downside.
            GL.SetRevertBackfacing(true);

            Vector4 cameraSpaceClipPlane = CameraUtility.CameraSpaceClipPlane(offscreenCamera, new Vector3(0.0f, transform.position.y, 0.0f), Vector3.up, 1.0f);
            Matrix4x4 projection = mainCamera.projectionMatrix;
            Matrix4x4 obliqueProjection = projection;

            offscreenCamera.fieldOfView = mainCamera.fieldOfView;
            offscreenCamera.aspect = mainCamera.aspect;

            CameraUtility.CalculateObliqueMatrix(ref obliqueProjection, cameraSpaceClipPlane);

            // Do the actual render, with the near plane set as the clipping plane. See the pro water source for details.
            offscreenCamera.projectionMatrix = obliqueProjection;

            if (!reflectionEnabled)
                offscreenCamera.cullingMask = 0;

            offscreenCamera.Render();

            GL.SetRevertBackfacing(false);

            // Refraction pass.
            offscreenCamera.cullingMask = cullingMask;
            offscreenCamera.targetTexture = refractionTexture;
            obliqueProjection = projection;
            offscreenCameraTransform.position = mainCameraTransform.position;
            offscreenCameraTransform.rotation = mainCameraTransform.rotation;
            offscreenCamera.worldToCameraMatrix = originalWorldToCamera;

            cameraSpaceClipPlane = CameraUtility.CameraSpaceClipPlane(offscreenCamera, Vector3.zero, Vector3.up, -1.0f);
            CameraUtility.CalculateObliqueMatrix(ref obliqueProjection, cameraSpaceClipPlane);
            offscreenCamera.projectionMatrix = obliqueProjection;
            offscreenCamera.Render();
            offscreenCamera.projectionMatrix = projection;
            offscreenCamera.targetTexture = null;
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
        /// Updates the wave.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        private IEnumerator UpdateWave()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                if (forceStorm)
                    humidity = FORCE_STORM_HUMIDITY;
                else
                    humidity = GetHumidity();

                waveScaleRealTime = Mathf.Lerp(0.0f, waveScale, humidity);
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
        /// Updates the light direction.
        /// </summary>
        private void UpdateLightDirection()
        {
            oceanMaterial.SetVector("_LightDir", lightDirection);
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
            int count = MAX_LOD;

            for (i = 0; i < count; ++i)
            {
                int idx = 0;
                double pow = Math.Pow(2, i);

                int length = (int)(tilePolygonHeight / pow + 1) * (int)(tilePolygonWidth / pow + 1);
                Vector3[] verticesLOD = new Vector3[length];
                Vector2[] uvLOD = new Vector2[length];

                for (y = 0; y < geometryHeight; y += (int)pow)
                {
                    for (x = 0; x < geometryWidth; x += (int)pow)
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
                double pow = Math.Pow(2, i);
                int widthLOD = (int)(tilePolygonWidth / pow + 1);
                int[] triangles = new int[(int)(tilePolygonHeight / pow * tilePolygonWidth / pow) * 6];

                int height = (int)(tilePolygonHeight / pow);
                int width = (int)(tilePolygonWidth / pow);

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

        /// <summary>
        /// This function return smooth random value from 0 to 1, used for smooth waves scale calculation ®"MindBlocks".
        /// </summary>
        /// <returns>System.Single.</returns>
        private float GetHumidity()
        {
            float time = Time.time;

            int intTime = (int)(time * HUMIDITY_UPDATE_FREQUENCY);
            int intPrevTime = (int)(prevHumidityUpdateTime * HUMIDITY_UPDATE_FREQUENCY);

            if (intTime != intPrevTime)
            {
                prevHumidityValue = nextHumidityValue;
                nextHumidityValue = UnityEngine.Random.value;
            }

            prevHumidityValue = time;
            float t = time * HUMIDITY_UPDATE_FREQUENCY - intTime;

            return Mathf.SmoothStep(prevHumidityValue, nextHumidityValue, t);
        }

        /// <summary>
        /// Destroys the reflection texture.
        /// </summary>
        private void DestroyReflectionTexture()
        {
            if (reflectionTexture != null)
                DestroyImmediate(reflectionTexture);

            reflectionTexture = null;
        }

        /// <summary>
        /// Destroys the refraction texture.
        /// </summary>
        private void DestroyRefractionTexture()
        {
            if (refractionTexture != null)
                DestroyImmediate(refractionTexture);

            refractionTexture = null;
        }
    }
}