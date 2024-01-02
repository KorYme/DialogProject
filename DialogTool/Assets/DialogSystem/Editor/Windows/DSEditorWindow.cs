using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        DSGraphData _graphDataSO;

        public DSGraphSaveHandler GraphSaveHandler { get; private set; }
        public Action<DSGraphData> LoadData { get; private set; }
        public Action SaveData { get; private set; }

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
            LoadInitialData();
            new DSGroup(CreateInstance<DSGroupData>());
        }

        private void OnDisable()
        {
            SaveData?.Invoke();
            SaveData = null;
            LoadData = null;
        }

        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView(this);
            graphView.StretchToParentSize();
            SaveData += graphView.SaveGraph;
            LoadData += graphView.LoadGraphData;
            LoadData += data => _graphDataSO = data;
            rootVisualElement.Add(graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();
            toolbar.Add(DSElementUtility.CreateObjectField("Graph File :", typeof(DSGraphData), _graphDataSO, 
                eventCallBack => LoadData?.Invoke(eventCallBack.newValue as DSGraphData)));
            toolbar.Add(DSElementUtility.CreateButton("Save", SaveData));
            toolbar.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSVariables.uss");
        }

        private void LoadInitialData()
        {
            if (_graphDataSO != null)
            {
                LoadData?.Invoke(_graphDataSO);
            }
        }
    }
}