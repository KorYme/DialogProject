using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace KorYmeLibrary.DialogueSystem.Utilities
{
    public class DSGraphSaveHandler
    {
        readonly static string FOLDER_PATH = Path.Combine("Assets", "DialogueGraphSaved");

        public DSGraphSaveHandler()
        {
            GenerateFolder();
        }

        public void GenerateFolder()
        {
            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(Path.Combine(FOLDER_PATH));
                AssetDatabase.Refresh();
            }
        }

        public void GenerateFile(string fileName)
        {
            if (!File.Exists(Path.Combine(FOLDER_PATH, fileName)))
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DSGraphData>(), Path.Combine(FOLDER_PATH, fileName) + ".asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
