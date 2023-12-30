using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSChoiceNode : DSNode
{
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

    protected override void DrawInputContainer()
    {
        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputPort.portName = "Dialogue Connection";
        inputContainer.Add(inputPort);
    }

    protected override void DrawMainContainer()
    {
        Button addChoiceButton = new Button()
        {
            text = "Add Choice",
        };
        addChoiceButton.clicked += () => Choices.Add("New choice");
        addChoiceButton.AddToClassList("ds-node__button");
        mainContainer.Insert(1, addChoiceButton);
    }

    protected override void DrawOutputContainer()
    {
        foreach (var choice in Choices)
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "";
            Button deleteChoiceButton = new Button()
            {
                text = "X",
            };
            deleteChoiceButton.clicked += () => Choices.Remove(choice);
            deleteChoiceButton.AddToClassList("ds-node__button");
            TextField choiceTextField = new TextField()
            {
                value = choice,
            };
            choiceTextField.AddToClassList("ds-node__textfield");
            choiceTextField.AddToClassList("ds-node__choice-textfield");
            choiceTextField.AddToClassList("ds-node__textfield_hidden");
            outputPort.Add(choiceTextField);
            outputPort.Add(deleteChoiceButton);
            outputContainer.Add(outputPort);
        }
    }

    protected override void DrawExtensionContainer()
    {
        VisualElement customDataContainer = new VisualElement();
        customDataContainer.AddToClassList("ds-node__custom-data-container");
        Foldout textFoldout = new Foldout()
        {
            text = "Dialogue Text",
        };
        TextField textTextField = new TextField()
        {
            value = Text,
        };
        textTextField.AddToClassList("ds-node__textfield");
        textTextField.AddToClassList("ds-node__quote-textfield");
        textFoldout.Add(textTextField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
    }
}
