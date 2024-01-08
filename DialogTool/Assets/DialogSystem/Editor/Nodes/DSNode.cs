using System;
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
        protected DSGraphView _graphView;
        private Action _onNodeDataChanged;

        public DSNode()
        {
            mainContainer.AddClasses("ds-node__main-container");
            extensionContainer.AddClasses("ds-node__extension-container");
        }

        public void InitializeElement(DSGraphView graphView, Vector2 position)
        {
            GenerateNodeData();
            InitializeNodeDataFields();
            _graphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
        }

        public void InitializeElement(DSGraphView graphView, DSNodeData data)
        {
            NodeData = data;
            _onNodeDataChanged?.Invoke();
            _graphView = graphView;
            SetPosition(new Rect(data.Position, Vector2.zero));
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
            _onNodeDataChanged += () => dataScriptable.SetValueWithoutNotify(NodeData);
            dataScriptable.SetEnabled(false);
            scriptableReferenceFoldout.Add(dataScriptable);
            mainContainer.Insert(1, scriptableReferenceFoldout);
        }

        protected virtual void DrawInputContainer() { }

        protected virtual void DrawOutputContainer() { }

        protected virtual void DrawExtensionContainer() { }

        public virtual void Save()
        {
            NodeData.Position = GetPosition().position;
        }

        #region UTILITIES
        protected void DisconnectAllPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                _graphView.DeleteElements(port.connections);
                port.DisconnectAll();
            }
        }
        #endregion
    }
}
