using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNodeData : DSNodeData
    {
        [TextArea(3,10)] public string DialogueText = "Random dialogue !?";
        public List<ChoicePortData> OutputNodes= new List<ChoicePortData>()
        {
            new ChoicePortData(),
        };
    }

    [System.Serializable]
    public class ChoicePortData
    {
        [SerializeField] public string ChoiceText;
        [SerializeField] public DSNodeData InputPortConnected;

        public ChoicePortData(string choiceName = "New Choice", DSNodeData outputPort = null)
        {
            ChoiceText = choiceName;
            InputPortConnected = outputPort;
        }
    }
}