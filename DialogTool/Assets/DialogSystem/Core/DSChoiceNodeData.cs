using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [field: SerializeField] public string DialogueText { get; set; } = "Random dialogue !?";
        [field: SerializeField] public List<ChoicePortData> OutputNodes { get; set; } = new List<ChoicePortData>()
        {
            ChoicePortData.Default,
            ChoicePortData.Default,
        };
    }

    [System.Serializable]
    public struct ChoicePortData
    {
        public string ChoiceText;
        public DSNodeData OutputPort;

        public static ChoicePortData Default => new ChoicePortData("New Choice", null);

        public ChoicePortData(string choiceName, DSNodeData outputPort)
        {
            ChoiceText = choiceName;
            OutputPort = outputPort;
        }
    }
}