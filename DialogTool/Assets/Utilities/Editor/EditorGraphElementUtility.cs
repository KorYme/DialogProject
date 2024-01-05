using UnityEditor.Experimental.GraphView;

namespace KorYmeLibrary.Utilities.Editor
{
    public static class EditorGraphElementUtility
    {
        public static Port CreatePort(this Node node, string name = null, string portName = null,
            Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.name = name;
            port.portName = portName;
            return port;
        }
    }
}