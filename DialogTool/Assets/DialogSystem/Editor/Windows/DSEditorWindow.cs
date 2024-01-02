using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        public DSGraphData GraphData { get; private set; }
        public string FileName { get; private set; } = "";
        DSGraphView _graphView;

        public DSGraphSaveHandler GraphSaveHandler { get; private set; }

        [MenuItem("Window/Dialog System/Dialogue Graph")]
        public static void OpenGraphWindow()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            GraphSaveHandler = new DSGraphSaveHandler();
            AddGraphView();
            AddToolbar();
            AddStyles();
            GenerateNewGraphView();
        }

        private void AddGraphView()
        {
            _graphView = new DSGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();
            toolbar.Add(DSElementUtility.CreateObjectField("Graph File :", typeof(DSGraphData), GraphData,
                eventCallBack =>
                {
                    GraphData = eventCallBack.newValue as DSGraphData;
                }));
            toolbar.Add(DSElementUtility.CreateButton("Save", () =>
            {
                if (GraphData != null)
                {
                    SaveData();
                }
                else
                {
                    Debug.LogWarning("There is no GraphData Loaded");
                }
            }));
            toolbar.Add(DSElementUtility.CreateButton("Load", GenerateNewGraphView));
            toolbar.Add(DSElementUtility.CreateTextField(FileName, "New File Name :", callbackEvent => FileName = callbackEvent.newValue));
            toolbar.Add(DSElementUtility.CreateButton("New Graph", () => GraphSaveHandler.GenerateGraphFile(FileName)));
            toolbar.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSVariables.uss");
        }

        private void SaveData()
        {
            _graphView?.SaveGraph();
        }

        private void LoadData()
        {
            _graphView?.LoadGraphData(GraphData);
        }

        private void GenerateNewGraphView()
        {
            _graphView?.ClearGraph();
            if (GraphData != null)
            {
                LoadData();
            }
        }
    }
}