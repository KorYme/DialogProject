using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroupData : DSElementData
    {
        [field: SerializeField] public List<DSNodeData> ChildrenNodes { get; set; } = new List<DSNodeData>();
    }
}