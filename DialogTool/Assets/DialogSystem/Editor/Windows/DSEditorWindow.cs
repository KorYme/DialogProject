using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DSEditorWindow : EditorWindow
{
    [MenuItem("Window/Dialog System/Dialog Graph")]
    public static void OpenGraphWindow()
    {
        GetWindow<DSEditorWindow>("Dialog Graph");
    }

    private void OnEnable()
    {
        AddGraphView();
    }

    private void AddGraphView()
    {
        DSGraphView graphView = new DSGraphView();

        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }
}
