using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [field:SerializeField , TextArea(3,10)] public string DialogueText { get; set; } = "Random dialogue !?";
        [field: SerializeField] public List<OutputPortData> OutputNodes { get; set; } = Enumerable.Repeat(new OutputPortData(), 2).ToList();
    }

    [System.Serializable]
    public class OutputPortData
    {
        [field: SerializeField] public string ChoiceText { get; set; }
        [field: SerializeField] public DSNodeData InputPortConnected { get; set; }

        public OutputPortData(string choiceName = "", DSNodeData outputPort = null)
        {
            ChoiceText = choiceName;
            InputPortConnected = outputPort;
        }
    }
}