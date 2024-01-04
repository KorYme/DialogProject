using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using KorYmeLibrary.DialogueSystem.Utilities;
using Unity.VisualScripting;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        DSDialogueGraphWindowData _dsDialogueGraphWindowData;
        public DSDialogueGraphWindowData WindowData
        {
            get
            {
                if (_dsDialogueGraphWindowData == null)
                {
                    _dsDialogueGraphWindowData = GraphSaveHandler.GetOrGenerateNewWindowData();
                }
                return _dsDialogueGraphWindowData;
            }
        }
        DSGraphView _graphView;
        DSGraphData _graphData;
        public DSGraphData GraphData
        {
            get => _graphData;
            set
            {
                if (value != _graphData)
                {
                    if (GraphData != null)
                    {
                        if (WindowData.IsSaveOnLoad)
                        {
                            SaveData();
                        }
                        GraphData.DeleteAllRemovedData();
                    }
                    _graphData = value;
                    _onGraphDataChange?.Invoke(value != null);
                    LoadData();
                }
            }
        }
        public DSGraphSaveHandler GraphSaveHandler { get; private set; }

        event Action<bool> _onGraphDataChange;
        event Action<string> _onFileNameChange;
        event Action<bool> _onMiniMapVisibilityChanged;

        [MenuItem("Window/Dialog System/Dialogue Graph")]
        public static void OpenGraphWindow()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            GraphSaveHandler = new DSGraphSaveHandler();
            _graphData = WindowData.LastGraphData;
            _onGraphDataChange += ChangeGraphDataInWindowData;
            AddGraphView();
            AddToolbar();
            AddStyles();
            LoadData();
        }

        private void OnDisable()
        {
            GraphData = null;
        }

        private void ChangeGraphDataInWindowData(bool graphDataIsNotNull)
        {
            if (graphDataIsNotNull)
            {
                WindowData.LastGraphData = GraphData;
            }
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
            Button clearButton = DSElementUtility.CreateButton("Clear", ClearGraph);
            Toggle autoSavetoggle = DSElementUtility.CreateToggle(WindowData.IsSaveOnLoad ,"Save on Load :", ChangeSaveOnLoad);
            TextField fileNameTextfield = DSElementUtility.CreateTextField(WindowData.FileName, "New File Name :", ChangeFileName);
            Button newGraphButton = DSElementUtility.CreateButton("New Graph", GenerateNewGraph);
            Button miniMapButton = DSElementUtility.CreateButton("Mini Map", ToggleMiniMap);

            saveButton.SetEnabled(GraphData != null);
            _onGraphDataChange += saveButton.SetEnabled;
            if (WindowData.IsMinimapVisible)
            {
                _graphView.ToggleMinimapVisibility();
                miniMapButton.AddClasses("ds-toolbar__button__selected");
            }
            _onMiniMapVisibilityChanged += x => miniMapButton.ToggleInClassList("ds-toolbar__button__selected");

            _onGraphDataChange += x => graphFileField.SetValueWithoutNotify(GraphData);
            _onFileNameChange += fileNameTextfield.SetValueWithoutNotify;

            toolbar.Add(graphFileField, saveButton, clearButton, autoSavetoggle, fileNameTextfield, newGraphButton, miniMapButton);
            toolbar.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

    #region TOOLBAR_METHODS
        private void ChangeGraphDataFile(ChangeEvent<UnityEngine.Object> callbackData) => GraphData = callbackData.newValue as DSGraphData;

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
            ClearGraph();
            _graphView?.LoadGraph(GraphData);
        }

        private void ClearGraph() => _graphView?.ClearGraph();
        
        private void ChangeSaveOnLoad(ChangeEvent<bool> callbackData) => WindowData.IsSaveOnLoad = callbackData.newValue;
        private void ChangeFileName(ChangeEvent<string> callbackData) => WindowData.FileName = callbackData.newValue;

        private void GenerateNewGraph()
        {
            if (!WindowData.FileName.IsSerializableFriendly())
            {
                Debug.LogWarning($"The file name \"{WindowData.FileName}\" could not be serialized, all non serializable characters have been removed.");
                WindowData.FileName = WindowData.FileName.RemoveNonSerializableCharacters();
                _onFileNameChange?.Invoke(WindowData.FileName);
                return;
            }
            DSGraphData newGraphData = GraphSaveHandler.GenerateGraphDataFile(WindowData.FileName);
            if (newGraphData != null)
            {
                if (GraphData != null)
                {
                    GraphData = newGraphData;
                }
                else
                {
                    _graphData = newGraphData;
                    _onGraphDataChange?.Invoke(true);
                    SaveData();
                }
            }
        }

        private void ToggleMiniMap()
        {
            WindowData.IsMinimapVisible = _graphView.ToggleMinimapVisibility();
            _onMiniMapVisibilityChanged?.Invoke(WindowData.IsMinimapVisible);
        }
    }
    #endregion
}