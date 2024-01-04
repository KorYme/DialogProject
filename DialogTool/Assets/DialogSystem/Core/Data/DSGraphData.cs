using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public partial class DSGraphData : ScriptableObject
    {
        [field: SerializeField] public List<DSNodeData> AllNodes { get; set; } = new List<DSNodeData>();
        [field: SerializeField] public List<DSGroupData> AllGroups { get; set; } = new List<DSGroupData>();
        
        #if UNITY_EDITOR
        [field: SerializeField] public List<DSElementData> AllRemovedElements { get; set; } = new List<DSElementData>();

        public void PlaceAllDataInRemoved()
        {
            AllRemovedElements.AddRange(AllNodes);
            AllRemovedElements.AddRange(AllGroups);
            AllNodes.Clear();
            AllGroups.Clear();
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