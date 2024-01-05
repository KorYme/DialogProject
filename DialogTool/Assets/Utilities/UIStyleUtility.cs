using UnityEngine.UIElements;

namespace KorYmeLibrary.Utilities
{
    public static class UIStyleUtility
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }
            return element;
        }
    }
}