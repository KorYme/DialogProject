using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSElementData : ScriptableObject
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
