using UnityEngine;
namespace Sources.MultipleClasses
{
    public class GridElementViewFactory : MonoBehaviour, IGridElementViewFactory
    {
        [SerializeField] private GridElementView _prefab;
        [SerializeField] private RectTransform _bounds;

        public IGridElementView CreateInstance(Grid grid)
        {
            GridElementView view = Instantiate(_prefab, _bounds);
            view.Initialize(grid, _bounds);
            return view;
        }
    }
}