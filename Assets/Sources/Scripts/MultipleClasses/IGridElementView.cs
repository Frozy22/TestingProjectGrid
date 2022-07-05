using UnityEngine;

namespace Sources.MultipleClasses
{
    public interface IGridElementView
    {
        public void ValueChanged(char value);
        public void SetPosition(Vector2Int to);
        public void ChangePosition(Vector2Int to);
        public void Dissolve();
    }
}