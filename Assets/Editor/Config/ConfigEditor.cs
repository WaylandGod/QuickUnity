using QuickUnityEditor;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The Config namespace.
/// </summary>
namespace QuickUnityEditor.Config
{
    /// <summary>
    /// Class ConfigEditor to edit config files.
    /// </summary>
    public sealed class ConfigEditor : Editor
    {
        /// <summary>
        /// The warning message about no config file.
        /// </summary>
        private const string WARNING_NO_CONFIG_FILE = "No config data files can be found !";

        /// <summary>
        /// The warning message about no config content can be found in file.
        /// </summary>
        private const string WARNING_NO_CONFIG_CONTENT = "No config content can be found in file !";

        /// <summary>
        /// The error message about the wrong format of config content.
        /// </summary>
        private const string ERROR_CONFIG_CONTENT_FORMAT_WRONG = "The format of config content was wrong !";

        /// <summary>
        /// The message about ConfigData files generated.
        /// </summary>
        private const string MESSAGE_GEN_SUCCESS = "ConfigData files generated !";

        /// <summary>
        /// The extensions of ConfigData file.
        /// </summary>
        private const string CONFIG_DATA_FILE_EXTENSIONS = ".cs";

        /// <summary>
        /// Generate Configuration ValueObject files.
        /// </summary>
        [MenuItem("QuickUnity/Config/Generate Configuration ValueObject files")]
        public static void GenerateConfigData()
        {
            string path = EditorUtility.OpenFolderPanel("Load csv config file of Directory", "Assets", "");

            // If path got nothing, do nothing.
            if (!QuickUnityEditorUtility.CheckAssetFilePath(path))
                return;

            string relativePath = QuickUnityEditorUtility.ConvertToRelativePath(path);
            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new string[1] { relativePath });

            if (guids.Length == 0)
            {
                Debug.LogWarning(WARNING_NO_CONFIG_FILE);
                return;
            }

            // Save asset file paths.
            string[] filePaths = new string[guids.Length];

            // Get asset file paths.
            for (int i = 0, length = guids.Length; i < length; ++i)
            {
                string guid = guids[i];
                filePaths[i] = AssetDatabase.GUIDToAssetPath(guid);
            }

            // Save all config data files.
            ArrayList assets = new ArrayList();

            foreach (string filePath in filePaths)
            {
                Object asset = AssetDatabase.LoadMainAssetAtPath(filePath);
                assets.Add(asset);
            }

            // Get template file content.
            string tplPath = EditorUtility.OpenFilePanel("Load ConfigData template file", "", "txt");

            if (!QuickUnityEditorUtility.CheckAssetFilePath(tplPath))
                return;

            string tplRelativePath = QuickUnityEditorUtility.ConvertToRelativePath(tplPath);
            TextAsset tplAsset = AssetDatabase.LoadMainAssetAtPath(tplRelativePath) as TextAsset;

            // Where you wanna save your ConfigData files.
            string configPath = EditorUtility.SaveFolderPanel("Save ConfigData files in the folder ...", "", "");

            if (!QuickUnityEditorUtility.CheckAssetFilePath(configPath))
                return;

            // Save ConfigData files.
            foreach (TextAsset textAsset in assets)
            {
                string assetPath = AssetDatabase.GetAssetPath(textAsset);
                string fileName = Path.GetFileNameWithoutExtension(assetPath);
                string savePath = configPath + Path.AltDirectorySeparatorChar + fileName + CONFIG_DATA_FILE_EXTENSIONS;
                string configDataContent = GetConfigDataContent(tplAsset.text, textAsset.text);
                configDataContent = configDataContent.Replace("$className$", fileName);
                StreamWriter writer = new StreamWriter(savePath, false);
                writer.Write(configDataContent);
                writer.Flush();
                writer.Close();
            }

            Debug.Log(MESSAGE_GEN_SUCCESS);

            // Collect garbage
            System.GC.Collect();
        }

        /// <summary>
        /// Gets the content of the ConfigData file.
        /// </summary>
        /// <param name="tplText">The text content of template file.</param>
        /// <param name="configText">The text content of configuration file.</param>
        /// <returns>System.String.</returns>
        private static string GetConfigDataContent(string tplText, string configText)
        {
            string[] configLines = configText.Split("\r\n"[0]);

            // Wrong format.
            if (configLines.Length < 3)
            {
                Debug.LogError(ERROR_CONFIG_CONTENT_FORMAT_WRONG);
                return "";
            }

            // No content.
            if (configLines.Length == 3)
            {
                Debug.LogWarning(WARNING_NO_CONFIG_CONTENT);
                return "";
            }

            // The fields defination of ConfigData.
            string fieldsStr = "";

            // Call methods of reading data.
            string methodsStr = "";

            string[] comments = configLines[0].Split(","[0]);
            string[] names = configLines[1].Split(","[0]);
            string[] types = configLines[2].Split(","[0]);

            // Loop name string array to generate replace string.
            for (int i = 0, length = names.Length; i < length; ++i)
            {
                string comment = comments[i];
                string name = names[i];
                string type = types[i];
                bool end = (i == length - 1);
                string fieldStr = GetConfigDataField(comment, name, type, end);
                string methodStr = GetConfigDataMethods(name, type);
                fieldsStr += fieldStr;
                methodsStr += methodStr;
            }

            tplText = tplText.Replace("$fields$", fieldsStr);
            tplText = tplText.Replace("$methods$", methodsStr);
            return tplText;
        }

        /// <summary>
        /// Gets the configuration data field.
        /// </summary>
        /// <param name="comment">The comment of field.</param>
        /// <param name="name">The name of field.</param>
        /// <param name="type">The type of field.</param>
        /// <param name="end">if set to <c>true</c> no need to make new line, else have to add line break.</param>
        /// <returns>System.String.</returns>
        private static string GetConfigDataField(string comment, string name, string type, bool end = false)
        {
            string fieldStr = "\t/// <summary>\r\n\t/// " + comment + "\r\n\t/// </summary>\r\n\t";
            fieldStr += "public " + type.Trim() + " " + name.Trim("\n"[0]) + ";";

            if (!end)
                fieldStr += "\r\n\r\n";

            return fieldStr;
        }

        /// <summary>
        /// Gets the methods of reading configuration data.
        /// </summary>
        /// <param name="name">The name of field.</param>
        /// <param name="type">The type of field.</param>
        /// <returns>System.String.</returns>
        private static string GetConfigDataMethods(string name, string type)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string methodStr = "\r\n\t\t" + name.Trim() + " = Read" + textInfo.ToTitleCase(type).Trim() + "(\"" + name.Trim() + "\");";
            return methodStr;
        }
    }
}