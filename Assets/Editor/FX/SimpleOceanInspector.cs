using QuickUnity.FX;
using System.Collections;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The FX namespace.
/// </summary>
namespace QuickUnityEditor.FX
{
    /// <summary>
    /// The Inspector view of SimpleOcean.
    /// </summary>
    [CustomEditor(typeof(SimpleOcean))]
    public class SimpleOceanInspector : Editor
    {
        /// <summary>
        /// Gets the ocean object.
        /// </summary>
        /// <value>The ocean.</value>
        private SimpleOcean ocean
        {
            get { return target as SimpleOcean; }
        }

        /// <summary>
        /// The texture settings foldout.
        /// </summary>
        private bool textureSettingsExpand = EditorPrefs.GetBool("textureSettingsExpand");

        /// <summary>
        /// The color settings foldout.
        /// </summary>
        private bool colorSettingsExpand = EditorPrefs.GetBool("colorSettingsExpand");

        /// <summary>
        /// The size settings foldout.
        /// </summary>
        private bool sizeSettingsExpand = EditorPrefs.GetBool("sizeSettingsExpand");

        /// <summary>
        /// The wave settings foldout.
        /// </summary>
        private bool waveSettingsExpand = EditorPrefs.GetBool("waveSettingsExpand");

        /// <summary>
        /// The other settings foldout.
        /// </summary>
        private bool otherSettingsExpand = EditorPrefs.GetBool("otherSettingsExpand");

        /// <summary>
        /// Called when [inspector GUI].
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUIUtility.LookLikeControls(80.0f, 40.0f);

            EditorGUILayout.Separator();
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUI.DropShadowLabel(rect, "Simple Ocean");
            GUILayout.Space(16);
            EditorGUILayout.EndVertical();

            // Texture Settings.
            textureSettingsExpand = EditorGUILayout.Foldout(textureSettingsExpand, "Texture Settings");
            EditorPrefs.SetBool("textureSettingsExpand", textureSettingsExpand);

            if (textureSettingsExpand)
            {
                EditorGUILayout.BeginVertical();

                // Normal map width and height.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Normal Map Width");
                GUILayout.Space(-20);
                ocean.normalMapWidth = EditorGUILayout.IntField(ocean.normalMapWidth, GUILayout.Width(40));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Normal Map Height");
                GUILayout.Space(-20);
                ocean.normalMapHeight = EditorGUILayout.IntField(ocean.normalMapHeight, GUILayout.Width(40));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                // Render Texture width and height.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Render Texture Width");
                GUILayout.Space(-20);
                ocean.renderTextureWidth = EditorGUILayout.IntField(ocean.renderTextureWidth, GUILayout.Width(40));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Render Texture Height");
                GUILayout.Space(-20);
                ocean.renderTextureHeight = EditorGUILayout.IntField(ocean.renderTextureHeight, GUILayout.Width(40));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }

            // Color Settings.
            colorSettingsExpand = EditorGUILayout.Foldout(colorSettingsExpand, "Color Settings");
            EditorPrefs.SetBool("colorSettingsExpand", colorSettingsExpand);

            if (colorSettingsExpand)
            {
                EditorGUILayout.BeginVertical();

                // Surface Color.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Surface Color");
                GUILayout.Space(-180);
                ocean.SurfaceColor = EditorGUILayout.ColorField(ocean.SurfaceColor);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                // Water Color.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Water Color");
                GUILayout.Space(-180);
                ocean.WaterColor = EditorGUILayout.ColorField(ocean.WaterColor);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                EditorGUILayout.EndVertical();
            }

            // Size Settings.
            sizeSettingsExpand = EditorGUILayout.Foldout(sizeSettingsExpand, "Size Settings");
            EditorPrefs.SetBool("sizeSettingsExpand", sizeSettingsExpand);

            if (sizeSettingsExpand)
            {
                EditorGUILayout.BeginVertical();

                // Tile polygon width.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Tile Polygon Width");
                GUILayout.Space(-20);
                ocean.tilePolygonWidth = EditorGUILayout.IntField(ocean.tilePolygonWidth, GUILayout.Width(40));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Tile Polygon Height");
                GUILayout.Space(-20);
                ocean.tilePolygonHeight = EditorGUILayout.IntField(ocean.tilePolygonHeight, GUILayout.Width(40));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                // Ocean Tile Size.
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Ocean Tile Size");
                ocean.oceanTileSize = EditorGUILayout.Vector3Field("", ocean.oceanTileSize);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }

            // Wave Settings.
            waveSettingsExpand = EditorGUILayout.Foldout(waveSettingsExpand, "Wave Settings");
            EditorPrefs.SetBool("waveSettingsExpand", waveSettingsExpand);

            if (waveSettingsExpand)
            {
                EditorGUILayout.BeginVertical();

                // Force Storm.
                ocean.forceStorm = EditorGUILayout.ToggleLeft(" Force Storm", ocean.forceStorm);
                EditorGUILayout.Separator();

                // Choopy Scale.
                EditorGUILayout.LabelField("Choopy Scale");
                ocean.choppyScale = EditorGUILayout.Slider(ocean.choppyScale, 0.1f, 10.0f);

                // Wave Scale Setting.
                EditorGUILayout.LabelField("Wave Scale");
                ocean.waveScale = EditorGUILayout.Slider(ocean.waveScale, 0.1f, 10.0f);
                EditorGUILayout.Separator();

                // Wave Speed Setting.
                EditorGUILayout.LabelField("Wave Speed");
                ocean.waveSpeed = EditorGUILayout.Slider(ocean.waveSpeed, 0.1f, 4.0f);
                EditorGUILayout.Separator();

                EditorGUILayout.EndVertical();
            }

            // Other Settings.
            otherSettingsExpand = EditorGUILayout.Foldout(otherSettingsExpand, "Other Settings");
            EditorPrefs.SetBool("otherSettingsExpand", otherSettingsExpand);

            if (otherSettingsExpand)
            {
                EditorGUILayout.BeginVertical();

                // Ocean Material.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ocean Material");
                GUILayout.Space(-180);
                ocean.oceanMaterial = (Material)EditorGUILayout.ObjectField(ocean.oceanMaterial, typeof(Material), true);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                // Render Layers.

                // Tiles Count.
                EditorGUILayout.LabelField("Tiles Count");
                ocean.tilesCount = (int)EditorGUILayout.Slider(ocean.tilesCount, 1, 10);
                EditorGUILayout.Separator();

                // Light Direction.
                EditorGUILayout.LabelField("Light Direction");
                ocean.lightDirection = EditorGUILayout.Vector3Field("", ocean.lightDirection);
                EditorGUILayout.Separator();

                // Reflection Enabled.
                ocean.reflectionEnabled = EditorGUILayout.ToggleLeft(" Reflection Enabled", ocean.reflectionEnabled);
                EditorGUILayout.Separator();

                EditorGUILayout.EndVertical();
            }
        }
    }
}