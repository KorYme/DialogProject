using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace KorYmeLibrary.Utilities.Editor
{
    public static class EditorUIElementUtility
    {
        public static ObjectField CreateObjectField(string title = null, Type type = null, UnityEngine.Object initialValue = null, EventCallback<ChangeEvent<UnityEngine.Object>> callback = null)
        {
            ObjectField objectField = new ObjectField()
            {
                label = title,
                objectType = type,
                value = initialValue,
            };
            if (callback != null)
            {
                objectField.RegisterValueChangedCallback(callback);
            }
            return objectField;
        }
    }
}
