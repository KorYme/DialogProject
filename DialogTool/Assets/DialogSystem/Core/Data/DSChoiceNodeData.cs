using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [field:SerializeField , TextArea(3,10)] public string DialogueText { get; set; } = "Random dialogue !?";
        [field: SerializeField] public List<PortData> OutputNodes { get; set; } = new List<PortData>()
        {
            new PortData(),
        };
    }

    [System.Serializable]
    public class PortData
    {
        [field: SerializeField] public string ChoiceText { get; set; }
        [field: SerializeField] public DSNodeData InputPortConnected { get; set; }

        public PortData(string choiceName = "New Choice", DSNodeData outputPort = null)
        {
            ChoiceText = choiceName;
            InputPortConnected = outputPort;
        }
    }
}