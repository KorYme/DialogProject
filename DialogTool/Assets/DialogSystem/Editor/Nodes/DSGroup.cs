using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroup : Group
    {
        public string ID { get; private set; }
        public DSGroup() => ID = Guid.NewGuid().ToString();

        public void RemoveAllSubElements() => RemoveElements(containedElements);
    }
}
