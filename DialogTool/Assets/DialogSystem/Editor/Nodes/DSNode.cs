using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using KorYmeLibrary.DialogueSystem.Utilities;
using KorYmeLibrary.DialogueSystem.Windows;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSNode : Node
    {
        public virtual string NodeName { get; set; }

        protected DSGraphView _graphView;

        public virtual void Initialize(DSGraphView graphView, Vector2 position)
        {
            NodeName = GetType().Name;
            _graphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddClasses("ds-node__main-container");
            extensionContainer.AddClasses("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            // TITLE CONTAINER
            DrawTitleContainer();

            // MAIN CONTAINER
            DrawMainContainer();

            // INPUT CONTAINER
            DrawInputContainer();

            // OUTPUT CONTAINER
            DrawOutputContainer();

            // EXTENSION CONTAINER
            DrawExtensionContainer();

            // USEFUL CALLS
            RefreshExpandedState();
        }

        protected virtual void DrawTitleContainer() 
        {
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(NodeName);
            titleContainer.Insert(0, dialogueNameTextField);
            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );
        }

        protected virtual void DrawMainContainer() { }

        protected virtual void DrawInputContainer()
        {
            Port inputPort = this.CreatePort("Dialogue Connection", direction: Direction.Input, capacity: Port.Capacity.Multi);
            inputContainer.Add(inputPort);
        }

        protected virtual void DrawOutputContainer() { }

        protected virtual void DrawExtensionContainer() { }

        public void DisconnectAllPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                _graphView.DeleteElements(port.connections);
                port.DisconnectAll();
            }
        }
    }
}
