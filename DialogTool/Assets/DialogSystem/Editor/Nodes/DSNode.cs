using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DSNode : Node
{
    public virtual string NodeName { get; set; }

    public virtual void Initialize(Vector2 position)
    {
        NodeName = "NodeName";
        SetPosition(new Rect(position, Vector2.zero));
        mainContainer.AddToClassList("ds-node__main-container");
        titleContainer.AddToClassList("ds");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        // TITLE CONTAINER
        DrawTitleContainer();

        // INPUT CONTAINER
        DrawInputContainer();

        // MAIN CONTAINER
        DrawMainContainer();

        // OUTPUT CONTAINER
        DrawOutputContainer();

        // EXTENSION CONTAINER
        DrawExtensionContainer();

        // USEFUL CALLS
        RefreshExpandedState();
    }

    protected virtual void DrawTitleContainer() 
    {
        TextField dialogueNameTextField = new TextField()
        {
            value = NodeName,
        };
        titleContainer.Insert(0, dialogueNameTextField);
        dialogueNameTextField.AddToClassList("ds-node__textfield");
        dialogueNameTextField.AddToClassList("ds-node__filename-textfield");
        dialogueNameTextField.AddToClassList("ds-node__textfield_hidden");
    }

    protected virtual void DrawInputContainer() { }

    protected virtual void DrawMainContainer() { }

    protected virtual void DrawOutputContainer() { }

    protected virtual void DrawExtensionContainer() { }
}
