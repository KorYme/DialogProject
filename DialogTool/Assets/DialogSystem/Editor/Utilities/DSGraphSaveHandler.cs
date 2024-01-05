using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using KorYmeLibrary.DialogueSystem.Windows;

namespace KorYmeLibrary.DialogueSystem.Utilities
{
    public class DSGraphSaveHandler
    {
        #region CONSTANTS
        readonly static string GRAPH_MAIN_FOLDER_PATH = Path.Combine("Assets", "DialogueGraphSaved");
        readonly static string GRAPH_WINDOW_DATA_FOLDER_PATH = Path.Combine(GRAPH_MAIN_FOLDER_PATH, "_DialogueGraphWindowDataFolder");
        #endregion

        #region CONSTRUCTOR
        public DSGraphSaveHandler()
        {
        }
        #endregion

        #region ASSET_CREATION_METHODS
        public void GenerateDSRootFolder()
        {
            if (!Directory.Exists(GRAPH_MAIN_FOLDER_PATH))
            {
                Directory.CreateDirectory(Path.Combine(GRAPH_MAIN_FOLDER_PATH));
            }
            if (!Directory.Exists(GRAPH_WINDOW_DATA_FOLDER_PATH))
            {
                Directory.CreateDirectory(Path.Combine(GRAPH_WINDOW_DATA_FOLDER_PATH));
                AssetDatabase.Refresh();
            }
        }

        public DSDialogueGraphWindowData GetOrGenerateNewWindowData()
        {
            GenerateDSRootFolder();
            string path = Path.Combine(GRAPH_WINDOW_DATA_FOLDER_PATH, "DialogueGraphWindowData") + ".asset";
            if (!File.Exists(path))
            {
                DSDialogueGraphWindowData windowGraphData = ScriptableObject.CreateInstance<DSDialogueGraphWindowData>();
                AssetDatabase.CreateAsset(windowGraphData, path);
                return windowGraphData;
            }
            else
            {
                return AssetDatabase.LoadAssetAtPath<DSDialogueGraphWindowData>(path);
            }
        }

        public DSGraphData GenerateGraphDataFile(string fileName)
        {
            if (fileName == "")
            {
                Debug.LogWarning("Please choose a valid name before generating a new graph file");
                return null;
            }
            GenerateDSRootFolder();
            if (!File.Exists(Path.Combine(GRAPH_MAIN_FOLDER_PATH, fileName) + ".asset"))
            {
                DSGraphData graphData = ScriptableObject.CreateInstance<DSGraphData>();
                AssetDatabase.CreateAsset(graphData, Path.Combine(GRAPH_MAIN_FOLDER_PATH, fileName) + ".asset");
                AssetDatabase.SaveAssets();
                if (!Directory.Exists(Path.Combine(GRAPH_MAIN_FOLDER_PATH, fileName)))
                {
                    Directory.CreateDirectory(Path.Combine(GRAPH_MAIN_FOLDER_PATH, fileName));
                    AssetDatabase.Refresh();
                }
                return graphData;
            }
            Debug.LogWarning("A file named the same way already exist, please rename it before generating a new graph");
            return null;
        }
        #endregion

        #region SAVE_AND_LOAD_UTILITIES
        public bool SaveDataInProject<T>(T elementData, string graphName) where T : DSElementData
        {
            Type tmpType = elementData.GetType();
            string path = "";
            while (tmpType != typeof(DSElementData))
            {
                path = Path.Combine(tmpType.Name, path);
                tmpType = tmpType.BaseType;
            }
            path = Path.Combine(GRAPH_MAIN_FOLDER_PATH, graphName, path);
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(Path.Combine(path));
                AssetDatabase.Refresh();
            }
            path = Path.Combine(path, elementData.ID) + ".asset";
            if (!File.Exists(path))
            {
                AssetDatabase.CreateAsset(elementData, path);
                AssetDatabase.SaveAssets();
                return true;
            }
            return false;
        }

        public void RemoveDataFromProject<T>(T elementData) where T : DSElementData
        {
            Type tmpType = typeof(T);
            string path = "";
            while (tmpType != typeof(DSElementData))
            {
                path = Path.Combine(tmpType.Name, path);
                tmpType = tmpType.BaseType;
            }
            path = Path.Combine(GRAPH_MAIN_FOLDER_PATH, path);
            if (!Directory.Exists(path))
            {
                Debug.Log("The file in which the data should have been saved has been destroyed");
            }
        }
        #endregion
    }
}
