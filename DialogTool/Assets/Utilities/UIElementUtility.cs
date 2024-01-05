using System;
using UnityEngine.UIElements;

namespace KorYmeLibrary.Utilities
{
    public static class UIElementUtility
    {
        public static VisualElement Add(this VisualElement element, params VisualElement[] childs)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                element.Add(childs[i]);
            }
            return element;
        }

        public static TextField CreateTextField(string initialValue = null, string labelValue = null, EventCallback<ChangeEvent<string>> onChangedCallback = null)
        {
            TextField textField = new TextField()
            {
                value = initialValue,
                label = labelValue,
            };
            if (onChangedCallback != null)
            {
                textField.RegisterValueChangedCallback(onChangedCallback);
            }
            return textField;
        }

        public static TextField CreateTextArea(string initialValue = null, string labelValue = null, EventCallback<ChangeEvent<string>> onChangedCallback = null)
        {
            TextField textField = CreateTextField(initialValue, labelValue, onChangedCallback);
            textField.multiline = true;
            return textField;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
            => new Foldout(){ text = title, value = !collapsed };

        public static Button CreateButton(string title, params Action[] onClickCallbacks)
        {
            Button button = new Button()
            {
                text = title
            };
            for (int i = 0; i < onClickCallbacks.Length; i++)
            {
                if (onClickCallbacks[i] == null) continue;
                button.clicked += onClickCallbacks[i];
            }
            return button;
        }

        public static Toggle CreateToggle(bool initialValue = false, string labelValue = null, EventCallback<ChangeEvent<bool>> callback = null)
        {
            Toggle toggle = new Toggle()
            {
                label = labelValue,
                value = initialValue,
                
            };
            toggle.RegisterValueChangedCallback(callback);
            return toggle;
        }
    }
}

