using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace KorYmeLibrary.DialogueSystem.Interfaces
{
    public interface IGraphSavable
    {
        void Save();
    }

    public interface IGraphInputable
    {
        Port InputPorts { get; }
        string ID { get; }
    }

    public interface IGraphOutputable
    {
        public void InitializeEdgeConnections(IEnumerable<IGraphInputable> inputables);
    }
}
