using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroupData : DSElementData
    {
        [SerializeField] public string Title = "New group";
        [SerializeField] public List<DSNodeData> ChildrenNodes = new List<DSNodeData>();
    }
}