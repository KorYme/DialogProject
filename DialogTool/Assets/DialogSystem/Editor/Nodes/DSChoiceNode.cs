using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Utilities;
using KorYmeLibrary.DialogueSystem.Windows;
using System;
using System.Linq;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNode : DSNode, IDSGraphSavable
    {
        public DSChoiceNodeData ChoiceNodeData => NodeData as DSChoiceNodeData;
        Action _savePortsAction = null;

        public DSChoiceNode(DSGraphView graphView, Vector2 position) : base(graphView, position)
        {
        }

        public DSChoiceNode(DSGraphView graphView, DSChoiceNodeData data) : base(graphView, data)
        {
        }

        protected override void InitializeData()
        {
            NodeData = ScriptableObject.CreateInstance<DSChoiceNodeData>();
            NodeData.ID = Guid.NewGuid().ToString();
        }

        protected override void DrawMainContainer()
        {
            base.DrawMainContainer();
            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () => CreateOutputPort());
            addChoiceButton.AddClasses("ds-node__button");
            mainContainer.Insert(2, addChoiceButton);
        }

        protected override void DrawOutputContainer()
        {
            foreach (var outputNode in ChoiceNodeData.OutputNodes)
            {
                CreateOutputPort(outputNode);
            }
        }

        protected override void DrawExtensionContainer()
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddClasses("ds-node__custom-data-container");
            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            TextField textTextField = DSElementUtility.CreateTextField(ChoiceNodeData.DialogueText, null, 
                callbackData => ChoiceNodeData.DialogueText = callbackData.newValue);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );
            textTextField.multiline = true;
            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        private Port CreateOutputPort(string choiceText = "New Choice")
        {
            ChoicePortData portData = new ChoicePortData(choiceText);
            ChoiceNodeData.OutputNodes.Add(portData);
            return CreateOutputPort(portData);
        }

        private Port CreateOutputPort(ChoicePortData choicePortData)
        {
            Port outputPort = this.CreatePort(choicePortData.InputPortConnected?.ID ?? null);
            Button deleteChoiceButton = DSElementUtility.CreateButton("X",
                () => RemoveChoicePort(outputPort),
                () => ChoiceNodeData.OutputNodes.Remove(choicePortData)
            );
            deleteChoiceButton.AddClasses("ds-node__button");
            TextField choiceTextField = DSElementUtility.CreateTextField(choicePortData.ChoiceText, null, callbackData =>
            {
                choicePortData.ChoiceText = callbackData.newValue;
            });
            _savePortsAction += () => choicePortData.InputPortConnected = (outputPort.connections?.FirstOrDefault()?.input.node as DSNode)?.NodeData ?? null;
            choiceTextField.AddClasses("ds-node__text-field", "ds-node__text-field__hidden", "ds-node__choice-text-field");
            outputPort.Add(deleteChoiceButton, choiceTextField);
            outputContainer.Add(outputPort);
            return outputPort;
        }

        public void InitializeEdgeConnections(IEnumerable<DSNode> nodes)
        {
            foreach (Port port in outputContainer.Children().OfType<Port>())
            {
                Port otherPort = nodes.FirstOrDefault(node => node.NodeData.ID == port.name)?.InputPort ?? null;
                if (otherPort is null) return;
                _graphView.AddElement(port.ConnectTo(otherPort));
            }
        }

        private void RemoveChoicePort(Port port)
        {
            Edge edge = port.connections.FirstOrDefault();
            if (edge != null)
            {
                _graphView.RemoveElement(edge);
            }
            port.DisconnectAll();
            outputContainer.Remove(port);
        }

        public override void Save()
        {
            base.Save();
            ChoiceNodeData.Position = NodeData.Position;
            ChoiceNodeData.NodeName = NodeData.NodeName;
            _savePortsAction?.Invoke();
        }
    }
}
