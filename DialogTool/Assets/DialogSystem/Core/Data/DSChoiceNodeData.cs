using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [field: SerializeField][field: TextArea(3,10)] public string DialogueText { get; set; } = "Random dialogue !?";
        [field: SerializeField] public List<ChoicePortData> OutputNodes { get; set; } = new List<ChoicePortData>()
        {
            new ChoicePortData(),
        };
    }

    [System.Serializable]
    public class ChoicePortData
    {
        [field: SerializeField] public string ChoiceText {  get; set; }
        [field: SerializeField] public DSNodeData InputPortConnected { get; set; }

        public ChoicePortData(string choiceName = "New Choice", DSNodeData outputPort = null)
        {
            ChoiceText = choiceName;
            InputPortConnected = outputPort;
        }
    }
}