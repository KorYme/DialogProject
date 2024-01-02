using Codice.CM.SEIDInfo;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
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

        public static Port CreatePort(this DSNode node, string otherNodeID = null, string portName = null, 
            Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, 
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.name = otherNodeID;
            port.portName = portName;
            return port;
        }

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
