using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
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
            AddGroupRenamedCallback();
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
            DSChoiceNode choiceNode = new DSChoiceNode(this, position);
            choiceNode.Draw();
            AddElement(choiceNode);
            return choiceNode;
        }

        public DSGroup CreateAndAddGroup(Vector2 position)
        {
            DSGroup group = new DSGroup(position)
            {
                title = "New_Group",
            };
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

        void AddGroupRenamedCallback()
        {
            groupTitleChanged = (group, text) => { };       
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
            nodeCreationRequest = context => OpenSearchWindow(context.screenMousePosition);
        }

        public bool OpenSearchWindow(Vector2 position) => SearchWindow.Open(new SearchWindowContext(GetLocalMousePosition(position)), _searchWindow);

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground()
            {
                name = "Background"
            };
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        #region UTILITIES
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false) => 
            contentContainer.WorldToLocal(mousePosition - (isSearchWindow ? _dsEditorWindow.position.position : Vector2.zero));
        #endregion

        #region SAVE_GRAPH
        public void LoadGraphData(DSGraphData graphData)
        {
            
        }

        public void SaveGraph()
        {
            foreach (GraphElement element in graphElements)
            {
                switch (element)
                {
                    case DSNode node: Debug.Log(node.NodeData.NodeName); break;
                    case DSGroup group: Debug.Log(group.title); break;
                    default: break;
                }
            }
        }
        #endregion
    }
}
