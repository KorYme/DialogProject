using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Utilities;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSGraphView : GraphView
    {
        DSSearchWindow _searchWindow;
        MiniMap _miniMap;
        DSEditorWindow _dsEditorWindow;

        IEnumerable<DSNode> _AllDSNodes => nodes.OfType<DSNode>();
        IEnumerable<DSChoiceNode> _AllDSChoiceNodes => nodes.OfType<DSChoiceNode>();

        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            _dsEditorWindow = dsEditorWindow;
            AddManipulators();
            AddSearchWindow();
            AddMinimap();
            AddStyles();
            AddMiniMapStyles();
            AddGraphViewChangeCallback();
            AddGridBackground();
        }

        private void AddMinimap()
        {
            _miniMap = new MiniMap()
            {
                anchored = true,
                visible = false,
            };
            _miniMap.SetPosition(new Rect(15, 50, 200, 125));
            Add(_miniMap);
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
            graphViewChanged = change =>
            {
                return change;
            };
        }

        private void AddStyles() => this.AddStyleSheets(
            "Assets/DialogSystem/Editor Default Resources/DSGraphViewStyles.uss",
            "Assets/DialogSystem/Editor Default Resources/DSNodeStyles.uss"
        );

        private void AddMiniMapStyles()
        {
            _miniMap.style.backgroundColor = new StyleColor(new Color32(29,29,29,255));
            _miniMap.style.borderBottomColor = new StyleColor(new Color32(51,51,51,255));
            _miniMap.style.borderTopColor = new StyleColor(new Color32(51,51,51,255));
            _miniMap.style.borderLeftColor = new StyleColor(new Color32(51,51,51,255));
            _miniMap.style.borderRightColor = new StyleColor(new Color32(51,51,51,255));
        }

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

        public void ClearGraph() => DeleteElements(graphElements);

        public bool ToggleMinimapVisibility()
        {
            _miniMap.visible = !_miniMap.visible;
            return _miniMap.visible;
        }

        #region UTILITIES
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false) => 
            contentContainer.WorldToLocal(mousePosition - (isSearchWindow ? _dsEditorWindow.position.position : Vector2.zero));
        #endregion

        #region SAVE_AND_LOAD_METHODS
        public void LoadGraph(DSGraphData graphData)
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
            ClearGraphData();
            IEnumerable<IDSGraphSavable> allElements = graphElements.OfType<IDSGraphSavable>();
            foreach (IDSGraphSavable element in allElements)
            {
                switch (element)
                {
                    case DSChoiceNode choiceNode:
                        AddToNodes(choiceNode.ChoiceNodeData);
                        break;
                    case DSGroup group:
                        AddToGroups(group.GroupData);
                        break;
                    default: break;
                }
                element.Save();
            }
        }

        void ClearGraphData() => _dsEditorWindow.GraphData.PlaceAllDataInRemoved();

        void SaveDataInProject<T>(T elementData) where T : DSElementData
            => _dsEditorWindow.GraphSaveHandler.SaveDataInProject(elementData, _dsEditorWindow.GraphData.name);

        void AddToNodes<T>(T nodeData) where T : DSNodeData
        {
            if (!_dsEditorWindow.GraphData.AllRemovedElements.Remove(nodeData))
            {
                SaveDataInProject(nodeData);
            }
            _dsEditorWindow.GraphData.AllNodes.Add(nodeData);
        }

        void AddToGroups<T>(T groupData) where T : DSGroupData
        {
            if (!_dsEditorWindow.GraphData.AllRemovedElements.Remove(groupData))
            {
                SaveDataInProject(groupData);
            }
            _dsEditorWindow.GraphData.AllGroups.Add(groupData);
        }
        #endregion
    }
}
