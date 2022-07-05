namespace Sources.MultipleClasses
{
    public interface IGridElementViewFactory
    {
        public IGridElementView CreateInstance(Grid grid);
    }
}