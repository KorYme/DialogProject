using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using Unity.VisualScripting;
using System.Drawing.Imaging;

namespace KorYmeLibrary.DialogueSystem.Utilities
{
    public class DSGraphSaveHandler
    {
        readonly static string FOLDER_PATH = Path.Combine("Assets", "DialogueGraphSaved");

        public DSGraphSaveHandler()
        {
        }

        public void GenerateDSRootFolder()
        {
            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(Path.Combine(FOLDER_PATH));
                AssetDatabase.Refresh();
            }
        }

        public DSGraphData GenerateGraphFile(string fileName)
        {
            if (fileName == "")
            {
                Debug.LogWarning("Please choose a valid name before generating a new graph file");
                return null;
            }
            GenerateDSRootFolder();
            if (!File.Exists(Path.Combine(FOLDER_PATH, fileName) + ".asset"))
            {
                DSGraphData graphData = ScriptableObject.CreateInstance<DSGraphData>();
                AssetDatabase.CreateAsset(graphData, Path.Combine(FOLDER_PATH, fileName) + ".asset");
                AssetDatabase.SaveAssets();
                if (!Directory.Exists(Path.Combine(FOLDER_PATH, fileName)))
                {
                    Directory.CreateDirectory(Path.Combine(FOLDER_PATH, fileName));
                    AssetDatabase.Refresh();
                }
                return graphData;
            }
            Debug.LogWarning("A file named the same way already exist, please rename it before generating a new graph");
            return null;
        }

        public bool SaveDataInProject<T>(T elementData, string graphName) where T : DSElementData
        {
            Type tmpType = typeof(T);
            string path = "";
            while (tmpType != typeof(DSElementData))
            {
                path = Path.Combine(tmpType.Name, path);
                tmpType = tmpType.BaseType;
            }
            path = Path.Combine(FOLDER_PATH, graphName, path);
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
            path = Path.Combine(FOLDER_PATH, path);
            if (!Directory.Exists(path))
            {
                Debug.Log("The file in which the data should have been saved has been destroyed");
            }
        }
    }
}
