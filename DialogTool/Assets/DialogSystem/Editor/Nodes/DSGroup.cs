using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroup : Group
    {
        public DSGroupData GroupData { get; protected set; }

        public DSGroup(Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));
            InitializeData();
        }

        public DSGroup(DSGroupData data)
        {
            SetPosition(new Rect(data.Position, Vector2.zero));
            GroupData = data;
        }

        private void InitializeData()
        {
            GroupData = ScriptableObject.CreateInstance<DSGroupData>();
            GroupData.ID = Guid.NewGuid().ToString();
        }

        public void RemoveAllSubElements() => RemoveElements(containedElements);
    }
}
