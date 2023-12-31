using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem.Utilities
{
    public static class DSElementUtility
    {
        public static VisualElement Add(this VisualElement element, params VisualElement[] childs)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                element.Add(childs[i]);
            }
            return element;
        }

        public static TextField CreateTextField(string initialValue = null, EventCallback<ChangeEvent<string>> onChangedCallback = null)
        {
            TextField textField = new TextField()
            {
                value = initialValue,
            };
            if (onChangedCallback != null)
            {
                textField.RegisterValueChangedCallback(onChangedCallback);
            }
            return textField;
        }

        public static TextField CreateTextArea(string initialValue = null, EventCallback<ChangeEvent<string>> onChangedCallback = null)
        {
            TextField textField = CreateTextField(initialValue, onChangedCallback);
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

        public static Port CreatePort(this DSNode node, string portName = "", 
            Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, 
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }
    }
}
