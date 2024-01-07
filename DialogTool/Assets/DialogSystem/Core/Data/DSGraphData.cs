using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGraphData : ScriptableObject
    {
        [field: SerializeField] public DSInitialNodeData InitialNode { get; set; } = null;
        [field: SerializeField] public List<DSNodeData> AllNodes { get; set; } = new List<DSNodeData>();
        [field: SerializeField] public List<DSGroupData> AllGroups { get; set; } = new List<DSGroupData>();
    }
}