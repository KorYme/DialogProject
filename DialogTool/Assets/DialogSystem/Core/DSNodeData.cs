using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSNodeData : DSElementData
    {
        [field: SerializeField] public string NodeName { get; set; } = "New Node";
    }
}