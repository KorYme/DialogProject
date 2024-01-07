using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [field:SerializeField , TextArea(3,10)] public string DialogueText { get; set; } = "Random dialogue !?";
        [field: SerializeField] public List<DSOutputPortData> OutputNodes { get; set; } = Enumerable.Repeat(new DSOutputPortData(), 2).ToList();
    }
}