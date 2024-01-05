using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Windows;
using KorYmeLibrary.DialogueSystem.Interfaces;
using KorYmeLibrary.Utilities;
using KorYmeLibrary.Utilities.Editor;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSNode : Node, IGraphSavable
    {
        public DSNodeData NodeData { get; protected set; }
        public Port InputPort { get; protected set; }
        protected DSGraphView _graphView;

        public DSNode() { }

        public void InitializeElement(DSGraphView graphView, Vector2 position)
        {
            GenerateNodeData();
            InitializeNodeDataFields();
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

        protected virtual void GenerateNodeData()
        {
            NodeData = ScriptableObject.CreateInstance<DSNodeData>();
        }

        protected virtual void InitializeNodeDataFields()
        {
            NodeData.ID = Guid.NewGuid().ToString();
            NodeData.ElementName = GetType().Name;
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

            // USEFUL CALL
            RefreshExpandedState();
        }

        protected virtual void DrawTitleContainer() 
        {
            TextField dialogueNameTextField = UIElementUtility.CreateTextField(NodeData.ElementName, null, 
                callbackEvent => NodeData.ElementName = callbackEvent.newValue);
            titleContainer.Insert(0, dialogueNameTextField);
            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );
        }

        protected virtual void DrawMainContainer()
        {
            Foldout scriptableReferenceFoldout = UIElementUtility.CreateFoldout("Scriptable Reference", true);
            ObjectField dataScriptable = EditorUIElementUtility.CreateObjectField(null, typeof(DSNodeData), NodeData);
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
