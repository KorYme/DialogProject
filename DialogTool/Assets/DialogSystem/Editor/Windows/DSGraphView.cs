using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void AddStyles()
    {
        StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/DialogSystem/Editor Default Resources/DSGraphViewStyles.uss");
        styleSheets.Add(graphViewStyleSheet);
        StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/DialogSystem/Editor Default Resources/DSNodeStyles.uss");
        styleSheets.Add(nodeStyleSheet);
    }

}
