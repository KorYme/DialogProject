using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public partial class DSGraphData : ScriptableObject
    {
        [SerializeField] public List<DSNodeData> AllNodes = new List<DSNodeData>();
        [SerializeField] public List<DSGroupData> AllGroups = new List<DSGroupData>();
        
        #if UNITY_EDITOR
        [SerializeField] public List<DSElementData> AllRemovedElements = new List<DSElementData>();

        public void PlaceAllDataInRemoved()
        {
            AllRemovedElements.AddRange(AllNodes);
            AllRemovedElements.AddRange(AllGroups);
            AllNodes.Clear();
            AllGroups.Clear();
            AllRemovedElements.RemoveAll(x => x is null);
        }

        public void DeleteAllRemovedData()
        {
            foreach (DSElementData elementData in AllRemovedElements)
            {
                if (elementData == null) continue;
                UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(elementData));
            }
            AllRemovedElements.Clear();
        }
        #endif
    }
}