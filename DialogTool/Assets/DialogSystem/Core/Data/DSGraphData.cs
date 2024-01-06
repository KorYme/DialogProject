using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGraphData : ScriptableObject
    {
        public List<DSNodeData> AllNodes = new List<DSNodeData>();
        public List<DSGroupData> AllGroups = new List<DSGroupData>();
    }
}