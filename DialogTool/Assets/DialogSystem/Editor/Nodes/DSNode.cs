using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using KorYmeLibrary.DialogueSystem.Utilities;
using KorYmeLibrary.DialogueSystem.Windows;
using System;
using UnityEditor.UIElements;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSNode : Node, IDSGraphSavable
    {
        public virtual DSNodeData NodeData { get; protected set; } 
        public Port InputPort {  get; protected set; }

        protected DSGraphView _graphView;

        public DSNode(DSGraphView graphView, Vector2 position)
        {
            InitializeData();
            _graphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddClasses("ds-node__main-container");
            extensionContainer.AddClasses("ds-node__extension-container");
        }

        public DSNode(DSGraphView graphView, DSNodeData data)
        {
            NodeData = data;
            _graphView = graphView;
            SetPosition(new Rect(data.Position, Vector2.zero));
            mainContainer.AddClasses("ds-node__main-container");
            extensionContainer.AddClasses("ds-node__extension-container");
        }

        protected virtual void InitializeData()
        {
            NodeData = ScriptableObject.CreateInstance<DSNodeData>();
            NodeData.ID = Guid.NewGuid().ToString();
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
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(NodeData.NodeName, null, 
                callbackEvent => NodeData.NodeName = callbackEvent.newValue);
            titleContainer.Insert(0, dialogueNameTextField);
            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );
        }

        protected virtual void DrawMainContainer()
        {
            ObjectField dataScriptable = DSElementUtility.CreateObjectField(null, typeof(DSNodeData), NodeData);
            dataScriptable.SetEnabled(false);
            mainContainer.Insert(1, dataScriptable);
        }

        protected virtual void DrawInputContainer()
        {
            InputPort = this.CreatePort(null, "Dialogue Connection", direction: Direction.Input, capacity: Port.Capacity.Multi);
            inputContainer.Add(InputPort);
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

        public void OpenSearchWindow() => _graphView.OpenSearchWindow(Vector2.zero);

        public virtual void Save()
        {
            NodeData.Position = GetPosition().position;
        }
    }
}
