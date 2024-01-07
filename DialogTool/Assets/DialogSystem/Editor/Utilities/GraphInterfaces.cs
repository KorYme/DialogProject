using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem.Interfaces
{
    public interface IGraphSavable
    {
        void Save();
    }

    public interface IGraphInputable
    {
        public VisualElement inputContainer { get; }
        Port InputPort { get; }
        string ID { get; }
        public void DisconnectAllInputPorts();
    }

    public interface IGraphOutputable
    {
        public VisualElement outputContainer { get; }
        public void InitializeEdgeConnections(IEnumerable<IGraphInputable> inputables);
        public void DisconnectAllOutputPorts();
    }
}
