using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSGraphView : GraphView
    {
        DSSearchWindow _searchWindow;
        DSEditorWindow _dsEditorWindow;

        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            _dsEditorWindow = dsEditorWindow;
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            AddStyles();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            //this.AddManipulator(GenerateContextMenuManipulator("Create Node/Choice Node", CreateAndAddChoiceNode));
            //this.AddManipulator(GenerateContextMenuManipulator("Create Group", CreateAndAddGroup));
        }

        public IManipulator GenerateContextMenuManipulator(string funcName, Func<Vector2, GraphElement> func)
            => new ContextualMenuManipulator(menuEvent => 
            menuEvent.menu.AppendAction(funcName, action => 
            func(GetLocalMousePosition(action.eventInfo.localMousePosition))));

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            DSNode node = evt.target as DSNode;
            if (node is not null)
            {
                evt.menu.AppendAction("Disconnect All Inputs Ports", action => node.DisconnectAllPorts(node.inputContainer));
                evt.menu.AppendAction("Disconnect All Output Ports", action => node.DisconnectAllPorts(node.outputContainer));
            }
            base.BuildContextualMenu(evt);
        }

        public DSChoiceNode CreateAndAddChoiceNode(Vector2 position)
        {
            DSChoiceNode choiceNode = new DSChoiceNode();
            choiceNode.Initialize(this, position);
            choiceNode.Draw();
            AddElement(choiceNode);
            return choiceNode;
        }

        public DSGroup CreateAndAddGroup(Vector2 position)
        {
            DSGroup group = new DSGroup()
            {
                title = "New Group",
            };
            group.SetPosition(new Rect(position, Vector2.zero));
            foreach (GraphElement selectedElement in selection.ToList())
            {
                switch (selectedElement)
                {
                    case DSNode:
                        group.AddElement(selectedElement); break;
                    default: break;
                }
            }
            AddElement(group);
            return group;
        }

        private void AddStyles() => this.AddStyleSheets(
            "Assets/DialogSystem/Editor Default Resources/DSGraphViewStyles.uss",
            "Assets/DialogSystem/Editor Default Resources/DSNodeStyles.uss"
        );

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
            => ports.Where(port => port.node != startPort.node && port.direction != startPort.direction).ToList();

        private void AddSearchWindow()
        {
            if (_searchWindow != null) return;
            _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
            _searchWindow.Initialize(this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(GetLocalMousePosition(context.screenMousePosition)), _searchWindow);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        #region UTILITIES
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false) => 
            contentContainer.WorldToLocal(mousePosition - (isSearchWindow ? _dsEditorWindow.position.position : Vector2.zero));
        #endregion
    }
}
