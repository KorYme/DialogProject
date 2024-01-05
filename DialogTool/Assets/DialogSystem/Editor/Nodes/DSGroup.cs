using System;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using KorYmeLibrary.DialogueSystem.Interfaces;
using KorYmeLibrary.DialogueSystem.Windows;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroup : Group, IGraphSavable
    {
        public DSGroupData GroupData { get; protected set; }

        public DSGroup() { }

        public void InitializeElement(DSGroupData groupData)
        {
            GroupData = groupData;
            InitializeFieldsWithGroupData();
        }

        public void InitializeElement(Vector2 position)
        {
            InitializeGroupData(position);
            InitializeFieldsWithGroupData();
        }

        protected virtual void InitializeGroupData(Vector2 position)
        {
            GroupData = ScriptableObject.CreateInstance<DSGroupData>();
            GroupData.ID = Guid.NewGuid().ToString();
            GroupData.Position = position;
        }

        void InitializeFieldsWithGroupData()
        {
            SetPosition(new Rect(GroupData.Position, Vector2.zero));
            title = GroupData.Title;
        }

        public void RemoveAllSubElements() => RemoveElements(containedElements);

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            base.OnGroupRenamed(oldName, newName);
            GroupData.Title = newName;
        }

        public void Save()
        {
            GroupData.Position = GetPosition().position;
            GroupData.ChildrenNodes = containedElements.OfType<DSNode>().Select(node => node.NodeData).ToList();
        }
    }
}
