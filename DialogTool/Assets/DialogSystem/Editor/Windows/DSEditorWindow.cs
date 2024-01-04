using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using System.Linq;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        DSGraphData _graphData;
        event Action<bool> _onGraphDataChange;
        public DSGraphData GraphData
        {
            get => _graphData;
            set
            {
                if (value != _graphData)
                {
                    GraphData?.DeleteAllRemovedData();
                    _graphData = value;
                    _onGraphDataChange?.Invoke(value != null);
                }
            }
        }

        event Action<string> _onFileNameChange;
        public string FileName { get; private set; }

        event Action<bool> _onMiniMapVisibilityChanged;

        DSGraphView _graphView;

        private bool _saveOnLoad;

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
            LoadData();
        }

        private void OnDisable()
        {
            GraphData?.DeleteAllRemovedData();
        }

        private void AddGraphView()
        {
            _graphView = new DSGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSVariables.uss");
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            ObjectField graphFileField = DSElementUtility.CreateObjectField("Graph File :", typeof(DSGraphData), GraphData, ChangeGraphDataFile);
            Button saveButton = DSElementUtility.CreateButton("Save", SaveData);
            Button loadButton = DSElementUtility.CreateButton("Load", LoadData);
            Button clearButton = DSElementUtility.CreateButton("Clear", ClearGraph);
            TextField fileNameTextfield = DSElementUtility.CreateTextField(FileName, "New File Name :", ChangeFileName);
            Button newGraphButton = DSElementUtility.CreateButton("New Graph", GenerateNewGraph);
            Button miniMapButton = DSElementUtility.CreateButton("Mini Map", ToggleMiniMap);

            saveButton.SetEnabled(GraphData != null);
            _onGraphDataChange += saveButton.SetEnabled;
            loadButton.SetEnabled(GraphData != null);
            _onGraphDataChange += loadButton.SetEnabled;
            _onMiniMapVisibilityChanged += x => miniMapButton.ToggleInClassList("ds-toolbar__button__selected");

            _onGraphDataChange += x => graphFileField.SetValueWithoutNotify(GraphData);
            _onFileNameChange += fileNameTextfield.SetValueWithoutNotify;

            toolbar.Add(graphFileField, saveButton, loadButton, clearButton, fileNameTextfield, newGraphButton, miniMapButton);
            toolbar.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

    #region TOOLBAR_METHODS
        private void ChangeGraphDataFile(ChangeEvent<UnityEngine.Object> callbackData)
        {
            if (true)
            {

            }
            GraphData = callbackData.newValue as DSGraphData;
        }

        private void SaveData()
        {
            if (GraphData != null)
            {
                _graphView?.SaveGraph();
            }
            else
            {
                Debug.LogWarning("There is no GraphData Loaded");
            }
        }

        private void LoadData()
        {
            _graphView?.ClearGraph();
            _graphView?.LoadGraph(GraphData);
        }

        private void ClearGraph()
        {
            _graphView?.ClearGraph();
        }

        private void ChangeFileName(ChangeEvent<string> callbackData)
        {
            FileName = callbackData.newValue;
        }

        private void GenerateNewGraph()
        {
            if (!FileName.IsSerializableFriendly())
            {
                Debug.LogWarning($"The file name \"{FileName}\" could not be serialized, all non serializable characters have been removed.");
                FileName = FileName.RemoveNonSerializableCharacters();
                _onFileNameChange?.Invoke(FileName);
                return;
            }
            DSGraphData newGraphData = GraphSaveHandler.GenerateGraphFile(FileName);
            if (newGraphData != null)
            {
                GraphData = newGraphData;
                LoadData();
            }
        }

        private void ToggleMiniMap()
        {
            bool minimapVisibility = _graphView.ToggleMinimapVisibility();
            _onMiniMapVisibilityChanged?.Invoke(minimapVisibility);
        }
    }
    #endregion
}