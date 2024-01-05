using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace KorYmeLibrary.DialogueSystem.Windows
{
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        #region FIELDS_AND_CONSTANTS
        const bool IS_WINDOW_CLOSED_ON_ENTRY_SELECTED = true;

        DSGraphView _dsGraphView;
        Texture2D _spacingIcon;
        #endregion

        #region METHODS
        public void Initialize(DSGraphView graphView)
        {
            _dsGraphView = graphView;
            _spacingIcon = new Texture2D(1, 1);
            _spacingIcon.SetPixel(0, 0, Color.clear);
            _spacingIcon.Apply();
        }
        #endregion

        #region INTERFACE_METHODS
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            return new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element"), 0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Choice Node", _spacingIcon))
                {
                    level = 2,
                    userData = nameof(DSChoiceNode),
                },
                new SearchTreeEntry(new GUIContent("Create Group", _spacingIcon))
                {
                    level = 1,
                    userData = nameof(DSGroup),
                },
            };
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            switch (SearchTreeEntry.userData)
            {
                case "DSChoiceNode": _dsGraphView.CreateAndAddNode<DSChoiceNode>(_dsGraphView.GetLocalMousePosition(context.screenMousePosition, true)); break;
                case "DSGroup": _dsGraphView.CreateAndAddGroup<DSGroup>(_dsGraphView.GetLocalMousePosition(context.screenMousePosition, true)); break;
                default: return false;
            }
            return IS_WINDOW_CLOSED_ON_ENTRY_SELECTED;
        }
        #endregion
    }
}
