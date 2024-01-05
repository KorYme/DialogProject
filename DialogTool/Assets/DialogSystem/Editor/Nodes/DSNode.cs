using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Utilities;
using KorYmeLibrary.DialogueSystem.Windows;
using KorYmeLibrary.DialogueSystem.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSNode : Node, IGraphSavable
    {
        public virtual DSNodeData NodeData { get; protected set; } 
        public Port InputPort {  get; protected set; }

        protected DSGraphView _graphView;        

        public DSNode(DSGraphView graphView, Vector2 position)
        {
            InitializeNodeData();
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

        public DSNode() { }

        public void InitializeElement(DSGraphView graphView, Vector2 position)
        {
            InitializeNodeData();
            _graphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddClasses("ds-node__main-container");
            extensionContainer.AddClasses("ds-node__extension-container");
        }

        public void InitializeElement(DSGraphView graphView, DSNodeData data)
        {
            NodeData = data;
            _graphView = graphView;
            SetPosition(new Rect(data.Position, Vector2.zero));
            mainContainer.AddClasses("ds-node__main-container");
            extensionContainer.AddClasses("ds-node__extension-container");
        }

        protected virtual void InitializeNodeData()
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
            Foldout scriptableReferenceFoldout = DSElementUtility.CreateFoldout("Scriptable Reference", true);
            ObjectField dataScriptable = DSElementUtility.CreateObjectField(null, typeof(DSNodeData), NodeData);
            dataScriptable.SetEnabled(false);
            scriptableReferenceFoldout.Add(dataScriptable);
            mainContainer.Insert(1, scriptableReferenceFoldout);
        }

        protected virtual void DrawInputContainer()
        {
            InputPort = this.CreatePort(NodeData.ID, "Dialogue Connection", direction: Direction.Input, capacity: Port.Capacity.Multi);
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

        public virtual void InitializeEdgeConnections(IEnumerable<DSNode> nodes)
        {
            foreach (Port port in outputContainer.Children().OfType<Port>())
            {
                Port otherPort = nodes.FirstOrDefault(node => node.NodeData.ID == port.name)?.InputPort ?? null;
                if (otherPort is null) return;
                _graphView.AddElement(port.ConnectTo(otherPort));
            }
        }

        public virtual void Save()
        {
            NodeData.Position = GetPosition().position;
        }
    }
}
