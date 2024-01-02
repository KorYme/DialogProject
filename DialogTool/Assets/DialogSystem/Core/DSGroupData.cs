using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroupData : DSElementData
    {
        [field: SerializeField] public string Title { get; set; } = "New group";
        [field: SerializeField] public List<DSNodeData> ChildrenNodes { get; set; } = new List<DSNodeData>();
    }
}