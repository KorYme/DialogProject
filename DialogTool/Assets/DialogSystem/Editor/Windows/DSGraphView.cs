using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSGraphView : GraphView
    {
        DSSearchWindow _searchWindow;
        DSEditorWindow _dsEditorWindow;

        IEnumerable<DSNode> _AllDSNodes => nodes.OfType<DSNode>();
        IEnumerable<DSChoiceNode> _AllDSChoiceNodes => nodes.OfType<DSChoiceNode>();

        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            _dsEditorWindow = dsEditorWindow;
            AddManipulators();
            AddSearchWindow();
            AddStyles();
            AddGraphViewChangeCallback();
            AddGridBackground();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
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

        public DSChoiceNode CreateAndAddChoiceNode(DSChoiceNodeData data)
        {
            DSChoiceNode choiceNode = new DSChoiceNode(this, data);
            choiceNode.Draw();
            AddElement(choiceNode);
            return choiceNode;
        }

        public DSGroup CreateAndAddGroup(Vector2 position, IEnumerable<GraphElement> allChildren = null)
        {
            DSGroup group = new DSGroup(position)
            {
                title = "New_Group",
            };
            group.AddElements((allChildren is null ? selection.OfType<DSNode>() : allChildren));
            AddElement(group);
            return group;
        }

        public DSGroup CreateAndAddGroup(DSGroupData data, IEnumerable<GraphElement> allChildren = null)
        {
            DSGroup group = new DSGroup(data)
            {
                title = "New_Group",
            };
            group.AddElements((allChildren is null ? selection.OfType<GraphElement>() : allChildren));
            AddElement(group);
            return group;
        }

        void AddGraphViewChangeCallback()
        {
            graphViewChanged = change => change;
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

        public void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground()
            {
                name = "Background"
            };
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        public void ClearGraph()
        {
            DeleteElements(graphElements);
        }

        #region UTILITIES
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false) => 
            contentContainer.WorldToLocal(mousePosition - (isSearchWindow ? _dsEditorWindow.position.position : Vector2.zero));
        #endregion

        #region SAVE_AND_LOAD_METHODS
        public void LoadGraphData(DSGraphData graphData)
        {
            if (graphData is null) return;
            // Generate all nodes
            foreach (DSNodeData nodeData in graphData.AllNodes)
            {
                switch (nodeData)
                {
                    case DSChoiceNodeData choiceNodeData:
                        CreateAndAddChoiceNode(choiceNodeData);
                        break;
                    default:
                        break;
                }
            }
            // Generate all groups
            foreach (DSGroupData group in graphData.AllGroups)
            {
                CreateAndAddGroup(group, _AllDSNodes.Where(dsNode => group.ChildrenNodes.Contains(dsNode.NodeData)));
            }
            // Link all Choice Nodes
            foreach (DSChoiceNode item in _AllDSChoiceNodes)
            {
                item.InitializeEdgeConnections(_AllDSNodes);
            }
            
        }

        public void SaveGraph()
        {
            foreach (IDSGraphSavable element in graphElements.OfType<IDSGraphSavable>())
            {
                switch (element)
                {
                    case DSChoiceNode choiceNode: 
                        SaveDataInProject(choiceNode.ChoiceNodeData);
                        AddToNodes(choiceNode.ChoiceNodeData);
                        break;
                    case DSGroup group: 
                        SaveDataInProject(group.GroupData); 
                        AddToGroups(group.GroupData);
                        break;
                    default: break;
                }
                element.Save();
            }
        }

        void SaveDataInProject<T>(T elementData) where T : DSElementData
            => _dsEditorWindow.GraphSaveHandler.SaveDataInProject(elementData, _dsEditorWindow.GraphData);

        void AddToNodes(DSNodeData nodeData)
        {
            if (_dsEditorWindow.GraphData.AllNodes.Contains(nodeData)) return;
            _dsEditorWindow.GraphData.AllNodes.Add(nodeData);
        }

        void AddToGroups(DSGroupData groupData)
        {
            if (_dsEditorWindow.GraphData.AllGroups.Contains(groupData)) return;
            _dsEditorWindow.GraphData.AllGroups.Add(groupData);
        }
        #endregion
    }
}
