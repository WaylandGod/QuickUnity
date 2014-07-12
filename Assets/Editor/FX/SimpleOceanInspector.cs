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
    //[CustomEditor(typeof(SimpleOcean))]
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
        /// The normal map settings foldout.
        /// </summary>
        private bool normalMapSettingsExpand;

        /// <summary>
        /// Called when [inspector GUI].
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUIUtility.LookLikeControls(80.0f);

            EditorGUILayout.Separator();
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUI.DropShadowLabel(rect, "Simple Ocean");
            GUILayout.Space(16);
            EditorGUILayout.EndVertical();

            normalMapSettingsExpand = EditorGUILayout.Foldout(normalMapSettingsExpand, "Normal Map Settings");

            if (normalMapSettingsExpand)
            {
                // Normal map texture size settings.
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Texture Width");
                GUILayout.Space(-40);
                ocean.normalMapWidth = EditorGUILayout.IntField(ocean.normalMapWidth);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Texture Height");
                GUILayout.Space(-40);
                ocean.normalMapHeight = EditorGUILayout.IntField(ocean.normalMapHeight);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}