using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    [System.Serializable]
    public class DSOutputPortData
    {
        [field: SerializeField] public string ChoiceText { get; set; }
        [field: SerializeField] public DSNodeData InputPortConnected { get; set; }

        public DSOutputPortData(string choiceName = "", DSNodeData outputPort = null)
        {
            ChoiceText = choiceName;
            InputPortConnected = outputPort;
        }
    }
}