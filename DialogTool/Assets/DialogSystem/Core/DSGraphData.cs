using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGraphData : ScriptableObject
    {
        [field: SerializeField] public List<DSNodeData> AllNodes { get; set; } 
        [field: SerializeField] public List<DSGroupData> AllGroups { get; set; }
    }
}
