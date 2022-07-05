using UnityEngine;

namespace Sources.MultipleClasses
{
    public struct GridElement
    {
        public char Value => _value;
        public Vector2Int Position => _position;
        public IGridElementView View { get; private set; }
        
        private char _value;
        private Vector2Int _position;

        public GridElement(char value, Vector2Int position)
        {
            _value = value;
            _position = position;
            View = null;
        }

        public void SetView(IGridElementView view)
        {
            View = view;
            view.ValueChanged(_value);
            view.SetPosition(_position);
        }
        
        public void ChangeValue(char value)
        {
            _value = value;
            View?.ValueChanged(_value);
        }

        public void SetPosition(Vector2Int to)
        {
            _position = to;
            View?.SetPosition(to);
        }

        public void ChangePosition(Vector2Int to)
        {
            _position = to;
            View?.ChangePosition(to);
        }
    }
}