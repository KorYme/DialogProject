using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSInitialNodeData : DSNodeData
    {
        [field: SerializeField] public DSOutputPortData OutputNode { get; set; } = new DSOutputPortData();
    }
}