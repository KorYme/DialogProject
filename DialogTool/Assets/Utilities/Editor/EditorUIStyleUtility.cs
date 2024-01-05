using UnityEditor;
using UnityEngine.UIElements;

namespace KorYmeLibrary.Utilities.Editor
{
    public static class EditorUIStyleUtility
    {
        public static VisualElement LoadAndAddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (string styleSheetName in styleSheetNames)
            {
                element.styleSheets.Add((StyleSheet)EditorGUIUtility.Load(styleSheetName));
            }
            return element;
        }
    }
}
