using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSElementData : ScriptableObject
    {
        [SerializeField] public string ElementName = "New Element";
        [SerializeField] public string ID;
        [SerializeField] public Vector2 Position;
    }
}
