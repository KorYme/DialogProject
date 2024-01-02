using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Utilities;
using KorYmeLibrary.DialogueSystem.Windows;
using System;
using Unity.VisualScripting;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNode : DSNode
    {
        public DSChoiceNodeData ChoiceNodeData => NodeData as DSChoiceNodeData;

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
            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () => CreateOutputPort());
            addChoiceButton.AddClasses("ds-node__button");
            mainContainer.Insert(1, addChoiceButton);
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
            TextField textTextField = DSElementUtility.CreateTextField(ChoiceNodeData.DialogueText);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );
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
            Port outputPort = this.CreatePort(choicePortData.ChoiceText);
            Button deleteChoiceButton = DSElementUtility.CreateButton("X",
                () => RemoveChoicePort(outputPort),
                () => ChoiceNodeData.OutputNodes.Remove(choicePortData)
            );
            deleteChoiceButton.AddClasses("ds-node__button");
            TextField choiceTextField = DSElementUtility.CreateTextField(outputPort.name, null, callbackData => outputPort.name = callbackData.newValue);
            choiceTextField.AddClasses("ds-node__text-field", "ds-node__text-field__hidden", "ds-node__choice-text-field");
            outputPort.Add(deleteChoiceButton, choiceTextField);
            outputContainer.Add(outputPort);
            return outputPort;
        }

        private void RemoveChoicePort(Port port)
        {
            outputContainer.Remove(port);
        }
    }
}
