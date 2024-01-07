using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.Utilities.Editor;
using KorYmeLibrary.DialogueSystem.Interfaces;
using KorYmeLibrary.Utilities;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSInitialNode : DSNode, IGraphOutputable
    {
        Action _savePortsAction = null;
        public DSInitialNodeData DerivedData => NodeData as DSInitialNodeData;

        Port OutputPort { get; set; }

        public DSInitialNode() : base() { }

        protected override void GenerateNodeData()
        {
            NodeData = ScriptableObject.CreateInstance<DSInitialNodeData>();
        }

        protected override void DrawOutputContainer()
        {
            OutputPort = CreateOutputPort(DerivedData.OutputNode);
        }

        protected Port CreateOutputPort(DSOutputPortData choicePortData)
        {
            Port outputPort = this.CreatePort(choicePortData.InputPortConnected?.ID ?? null);
            _savePortsAction += () => choicePortData.InputPortConnected = (outputPort.connections?.FirstOrDefault()?.input.node as DSNode)?.NodeData ?? null;
            Label choiceTextField = new Label("First Dialogue");
            outputPort.Add(choiceTextField);
            outputContainer.Add(outputPort);
            return outputPort;
        }

        public void InitializeEdgeConnections(IEnumerable<IGraphInputable> inputables)
        {
            Port otherPort = inputables.FirstOrDefault(inputable => inputable.ID == OutputPort.name)?.InputPort ?? null;
            if (otherPort is null) return;
            _graphView.AddElement(OutputPort.ConnectTo(otherPort));
        }

        public void DisconnectAllOutputPorts() => DisconnectAllPorts(outputContainer);

        public override void Save()
        {
            base.Save();
            _savePortsAction?.Invoke();
        }
    }
}
