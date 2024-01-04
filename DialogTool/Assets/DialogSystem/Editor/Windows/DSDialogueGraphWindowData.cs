using UnityEngine;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSDialogueGraphWindowData : ScriptableObject
    {
        [field: SerializeField] public bool IsSaveOnLoad { get; set; } = false;
        [field: SerializeField] public bool IsMinimapVisible { get; set; } = false;
        [field: SerializeField] public string FileName { get; set; } = "";
        [field: SerializeField] public DSGraphData LastGraphData { get; set; } = null;
    }
}
