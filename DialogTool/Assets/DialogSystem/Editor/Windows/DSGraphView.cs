using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(GenerateContextMenuManipulator("Create Node/Choice Node", CreateChoiceNode));
            this.AddManipulator(GenerateContextMenuManipulator("Create Group", CreateGroup));
        }

        public IManipulator GenerateContextMenuManipulator(string funcName, Func<Vector2, GraphElement> func)
            => new ContextualMenuManipulator(menuEvent => menuEvent.menu.AppendAction(funcName, action => AddElement(func(action.eventInfo.localMousePosition))));

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private DSChoiceNode CreateChoiceNode(Vector2 position)
        {
            DSChoiceNode choiceNode = new DSChoiceNode();
            choiceNode.Initialize(position);
            choiceNode.Draw();
            return choiceNode;
        }

        private Group CreateGroup(Vector2 position)
        {
            Group group = new Group()
            {
                title = "New Group",
            };
            group.SetPosition(new Rect(position, Vector2.zero));
            return group;
        }

        private void AddStyles() => this.AddStyleSheets(
            "Assets/DialogSystem/Editor Default Resources/DSGraphViewStyles.uss",
            "Assets/DialogSystem/Editor Default Resources/DSNodeStyles.uss"
        );

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
            => ports.Where(port => port.node != startPort.node && port.direction != startPort.direction).ToList();
    }
}
