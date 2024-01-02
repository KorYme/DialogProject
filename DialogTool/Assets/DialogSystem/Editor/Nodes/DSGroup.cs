using KorYmeLibrary.DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KorYmeLibrary.DialogueSystem
{
    public class DSGroup : Group, IDSGraphSavable
    {
        public DSGroupData GroupData { get; protected set; }

        public DSGroup(Vector2 position)
        {
            InitializeGroupData();
            InitializeData();
        }

        public DSGroup(DSGroupData data)
        {
            GroupData = data;
            InitializeData();
        }
        
        private void InitializeGroupData()
        {
            GroupData = ScriptableObject.CreateInstance<DSGroupData>();
            GroupData.ID = Guid.NewGuid().ToString();
        }

        void InitializeData()
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
