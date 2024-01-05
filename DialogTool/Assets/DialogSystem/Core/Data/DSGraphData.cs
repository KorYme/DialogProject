using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public partial class DSGraphData : ScriptableObject
    {
        [SerializeField] public List<DSNodeData> AllNodes = new List<DSNodeData>();
        [SerializeField] public List<DSGroupData> AllGroups = new List<DSGroupData>();
    }
}