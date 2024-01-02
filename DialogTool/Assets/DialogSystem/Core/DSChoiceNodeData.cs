using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [field: SerializeField] public string DialogueText { get; set; } = "Random dialogue !?";
        [field: SerializeField]
        public List<ChoicePortData> OutputNodes { get; set; } = new List<ChoicePortData>()
        {
            new ChoicePortData(),
        };
    }

    [System.Serializable]
    public class ChoicePortData
    {
        public string ChoiceText;
        public DSNodeData InputPortConnected;

        public ChoicePortData(string choiceName = "New Choice", DSNodeData outputPort = null)
        {
            ChoiceText = choiceName;
            InputPortConnected = outputPort;
        }
    }
}