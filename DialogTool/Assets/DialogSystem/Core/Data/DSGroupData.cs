using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroupData : DSElementData
    {
        [SerializeField] public List<DSNodeData> ChildrenNodes = new List<DSNodeData>();
    }
}