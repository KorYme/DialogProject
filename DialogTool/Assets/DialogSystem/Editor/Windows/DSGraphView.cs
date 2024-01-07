using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.Utilities;
using KorYmeLibrary.Utilities.Editor;
using KorYmeLibrary.DialogueSystem.Interfaces;
using Unity.VisualScripting;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSGraphView : GraphView
    {
        #region PROPERTIES_AND_FIELDS
        DSSearchWindow _searchWindow;
        MiniMap _miniMap;
        DSEditorWindow _dsEditorWindow;

        IEnumerable<DSNode> _AllDSNodes => nodes.OfType<DSNode>();
        DSInitialNode _initialNode;
        #endregion

        #region CONSTRUCTOR
        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            _dsEditorWindow = dsEditorWindow;
            AddManipulators();
            AddSearchWindow();
            AddMinimap();
            AddStyles();
            AddMiniMapStyles();
            AddGridBackground();
            _initialNode = CreateAndAddNode<DSInitialNode>(Vector2.zero);
        }
        #endregion

        #region MAIN_ELEMENTS_METHODS
        protected void AddMinimap()
        {
            _miniMap = new MiniMap()
            {
                anchored = true,
                visible = false,
            };
            _miniMap.SetPosition(new Rect(15, 50, 200, 125));
            Add(_miniMap);
        }

        public bool ToggleMinimapVisibility()
        {
            _miniMap.visible = !_miniMap.visible;
            return _miniMap.visible;
        }

        protected void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        protected void AddSearchWindow()
        {
            if (_searchWindow != null) return;
            _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
            _searchWindow.Initialize(this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

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
            _initialNode = CreateAndAddNode<DSInitialNode>(Vector2.zero);
        }
        #endregion

        #region CONTEXT_MENU_METHODS
        public IManipulator GenerateContextMenuManipulator(string funcName, Func<Vector2, GraphElement> func)
            => new ContextualMenuManipulator(menuEvent => 
            menuEvent.menu.AppendAction(funcName, action => 
            func(GetLocalMousePosition(action.eventInfo.localMousePosition))));

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            switch (evt.target)
            {
                case DSNode node:
                    if (node is IGraphInputable) evt.menu.AppendAction("Disconnect All Inputs Ports", action => (node as IGraphInputable).DisconnectAllInputPorts());
                    if (node is IGraphOutputable) evt.menu.AppendAction("Disconnect All Outputs Ports", action => (node as IGraphOutputable).DisconnectAllOutputPorts());
                    break;
                default:
                    break;
            }
            base.BuildContextualMenu(evt);
        }
        #endregion

        #region NODES_AND_GROUPS_CREATION_METHODS
        public T CreateAndAddNode<T>(Vector2 position) where T : DSNode, new()
        {
            T node = new T();
            node.InitializeElement(this, position);
            node.Draw();
            AddElement(node);
            return node;
        }

        public T CreateAndAddNode<T,Y>(Y data) where T: DSNode, new() where Y : DSNodeData, new()
        {
            T node = new T();
            node.InitializeElement(this, data);
            node.Draw();
            AddElement(node);
            return node;
        }

        public T CreateAndAddGroup<T>(Vector2 position, IEnumerable<GraphElement> allChildren = null) where T : DSGroup, new()
        {
            T group = new T();
            group.InitializeElement(position);
            group.AddElements((allChildren is null ? selection.OfType<DSNode>() : allChildren));
            AddElement(group);
            return group;
        }

        public T CreateAndAddGroup<T>(DSGroupData data, IEnumerable<GraphElement> allChildren = null) where T : DSGroup, new()
        {
            T group = new T();
            group.InitializeElement(data);
            group.AddElements((allChildren is null ? selection.OfType<GraphElement>() : allChildren));
            AddElement(group);
            return group;
        }
        #endregion

        #region SAVE_AND_LOAD_METHODS
        public void LoadGraph(DSGraphData graphData)
        {
            if (graphData == null) return;
            // Generate all nodes
            graphData.AllGroups.RemoveAll(x => x == null);
            graphData.AllNodes.RemoveAll(x => x == null);
            if (graphData.InitialNode != null)
            {
                _initialNode.InitializeElement(this, graphData.InitialNode);
            }
            foreach (DSNodeData nodeData in graphData.AllNodes)
            {
                switch (nodeData)
                {
                    case DSChoiceNodeData choiceNodeData:
                        CreateAndAddNode<DSChoiceNode, DSChoiceNodeData>(choiceNodeData);
                        break;
                    default:
                        CreateAndAddNode<DSNode, DSNodeData>(nodeData);
                        break;
                }
            }
            // Generate all groups
            foreach (DSGroupData group in graphData.AllGroups)
            {
                CreateAndAddGroup<DSGroup>(group, _AllDSNodes.Where(dsNode => group.ChildrenNodes.Contains(dsNode.NodeData)));
            }
            // Link all nodes
            foreach (IGraphOutputable outputable in nodes.OfType<IGraphOutputable>())
            {
                outputable.InitializeEdgeConnections(nodes.OfType<IGraphInputable>());
            }
        }

        public void SaveGraph(DSGraphData graphData)
        {
            if (graphData == null) return;
            // Place all nodes in the ready-to-remove-list
            List<DSElementData> allRemovedData = new List<DSElementData>();
            allRemovedData.AddRange(graphData.AllNodes);
            allRemovedData.AddRange(graphData.AllGroups);
            graphData.AllNodes.Clear();
            graphData.AllGroups.Clear();
            allRemovedData.RemoveAll(x => x == null);
            // Check if the node still exist or if it needs to be instantiated as a scriptable object
            IEnumerable<IGraphSavable> allElements = graphElements.OfType<IGraphSavable>();
            foreach (IGraphSavable element in allElements)
            {
                switch (element)
                {
                    case DSInitialNode initialNode:
                        if (graphData.InitialNode == null)
                        {
                            SaveDataInProject(initialNode.NodeData, graphData);
                        }
                        graphData.InitialNode = _initialNode.DerivedData;
                        EditorUtility.SetDirty(initialNode.NodeData);
                        break;
                    case DSNode node:
                        AddToNodes(node.NodeData, graphData, allRemovedData);
                        break;
                    case DSGroup group:
                        AddToGroups(group.GroupData, graphData, allRemovedData);
                        break;
                    default: 
                        break;
                }
                element.Save();
            }
            // Delete all the nodes which doesn't exist anymore on the graph
            foreach (DSElementData elementData in allRemovedData)
            {
                if (elementData == null) continue;
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(elementData));
            }
        }

        protected void SaveDataInProject<T>(T elementData, DSGraphData graphData) where T : DSElementData
            => _dsEditorWindow.GraphSaveHandler.SaveDataInProject(elementData, graphData.name);

        protected void AddToNodes<T>(T nodeData, DSGraphData graphData, List<DSElementData> allRemovedData) where T : DSNodeData
        {
            if (!allRemovedData.Remove(nodeData))
            {
                SaveDataInProject(nodeData, graphData);
            }
            graphData.AllNodes.Add(nodeData);
            EditorUtility.SetDirty(nodeData);
        }

        protected void AddToGroups<T>(T groupData, DSGraphData graphData, List<DSElementData> allRemovedData) where T : DSGroupData
        {
            if (!allRemovedData.Remove(groupData))
            {
                SaveDataInProject(groupData, graphData);
            }
            graphData.AllGroups.Add(groupData);
            EditorUtility.SetDirty(groupData);
        }
        #endregion

        #region STYLES_ADDITION_METHODS
        protected void AddStyles() => this.LoadAndAddStyleSheets(
            "DialogueSystem/DSGraphViewStyles.uss",
            "DialogueSystem/DSNodeStyles.uss"
        );

        protected void AddMiniMapStyles()
        {
            _miniMap.style.backgroundColor = new StyleColor(new Color32(29,29,29,255));
            _miniMap.style.borderBottomColor = new StyleColor(new Color32(51,51,51,255));
            _miniMap.style.borderTopColor = new StyleColor(new Color32(51,51,51,255));
            _miniMap.style.borderLeftColor = new StyleColor(new Color32(51,51,51,255));
            _miniMap.style.borderRightColor = new StyleColor(new Color32(51,51,51,255));
        }
        #endregion

        #region OVERRIDED_METHODS
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
            => ports.Where(port => port.node != startPort.node && port.direction != startPort.direction).ToList();
        #endregion

        #region UTILITIES
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            if (isSearchWindow)
            {
                worldMousePosition = _dsEditorWindow.rootVisualElement.ChangeCoordinatesTo(
                    _dsEditorWindow.rootVisualElement.parent, mousePosition - _dsEditorWindow.position.position) + Vector2.down * 20;
            }
            return contentViewContainer.WorldToLocal(worldMousePosition);
        }
        #endregion
    }
}
