using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Utilities;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSChoiceNode : DSNode
    {
        public override string NodeName { get; set; }
        public virtual List<string> Choices { get; set; }
        public virtual string Text { get; set; }
    
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            Choices = new List<string>
            {
                "New Choixe",
                "New Choife"
            };
            Text = "Dialogue Text";
        }

        protected override void DrawMainContainer()
        {
            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                Port outputPort = CreateChoicePort("New Choice");
                Choices.Add("New Choice");
                outputContainer.Add(outputPort);
            });
            addChoiceButton.AddClasses("ds-node__button");
            mainContainer.Insert(1, addChoiceButton);
        }

        protected override void DrawOutputContainer()
        {
            foreach (var choice in Choices)
            {
                Port outputPort = CreateChoicePort(choice);
                outputContainer.Add(outputPort);
            }
        }

        protected override void DrawExtensionContainer()
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddClasses("ds-node__custom-data-container");
            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            TextField textTextField = DSElementUtility.CreateTextField(Text);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );
            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        private Port CreateChoicePort(string choice)
        {
            Port outputPort = this.CreatePort();
            Button deleteChoiceButton = DSElementUtility.CreateButton("X", () => RemoveChoicePort(outputPort, choice));
            deleteChoiceButton.AddClasses("ds-node__button");
            TextField choiceTextField = DSElementUtility.CreateTextField(choice);
            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );
            outputPort.Add(deleteChoiceButton, choiceTextField);
            return outputPort;
        }

        private void RemoveChoicePort(Port port, string choice)
        {
            Choices.Remove(choice);
            outputContainer.Remove(port);
        }
    }
}
