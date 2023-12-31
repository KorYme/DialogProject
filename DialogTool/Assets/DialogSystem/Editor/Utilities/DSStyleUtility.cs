using UnityEditor;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem.Utilities
{
    public static class DSStyleUtility
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }
            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (string styleSheetName in styleSheetNames)
            {
                element.styleSheets.Add((StyleSheet)EditorGUIUtility.Load(styleSheetName));
            }
            return element;
        }
    }
}
