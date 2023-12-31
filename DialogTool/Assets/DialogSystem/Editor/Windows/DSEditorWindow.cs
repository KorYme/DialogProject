using KorYmeLibrary.DialogueSystem.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Window/Dialog System/Dialogue Graph")]
        public static void OpenGraphWindow()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }

        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Assets/DialogSystem/Editor Default Resources/DSVariables.uss");
        }
    }
}