using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.MultipleClasses
{
    public class Grid
    {
        private GridElement[,] _elements;
        public readonly int CountX;
        public readonly int CountY;
        private IGridElementViewFactory _viewFactory;
        private const string _symbols = "QWERTYUIOPASDFGHJKLZXCVBNM";

        public Grid(int countX, int countY, IGridElementViewFactory viewFactory)
        {
            CountX = countX;
            CountY = countY;
            _elements = new GridElement[CountX, CountY];
            _viewFactory = viewFactory;

            for (int x = 0; x < countX; x++)
            {
                for (int y = 0; y < countY; y++)
                {
                    _elements[x, y] = CreateElement(x, y, GetRandomValue());
                }
            }
        }

        private char GetRandomValue() => _symbols[Random.Range(0, _symbols.Length)];
        
        private GridElement CreateElement(int posX, int posY, char value)
        {
            GridElement element = new GridElement(value, new Vector2Int(posX, posY));
            if (_viewFactory != null)
                element.SetView(_viewFactory.CreateInstance(this));
            return element;
        }

        private bool ContainsPosition(int posX, int posY) 
            => posX >= 0 && posX < CountX && posY >= 0 && posY < CountY;

        public void SwapElements(int aPosX, int aPosY, int bPosX, int bPosY)
        {
            if (ContainsPosition(aPosX, aPosY) == false)
                throw new ArgumentOutOfRangeException("Grid Swap Elements: First element");
            if (ContainsPosition(bPosX, bPosY) == false)
                throw new ArgumentOutOfRangeException("Grid Swap Elements: Second element");

            var temp = _elements[aPosX, aPosY];
            _elements[aPosX, aPosY] = _elements[bPosX, bPosY];
            _elements[bPosX, bPosY] = temp;
        }

        public void ShuffleElements()
        {
            List<Vector2Int> places = new List<Vector2Int>();
            for (int x = 0; x < CountX; x++)
            for (int y = 0; y < CountY; y++)
            {
                places.Add(new Vector2Int(x, y));
            }

            GridElement redoElement = _elements[0, 0];
            Vector2Int place = Vector2Int.zero;
            places.Remove(place);
            while (true)
            {
                Vector2Int newPlace;
                int j = 0;
                if (places.Count > 0)
                {
                    do newPlace = places[Random.Range(0, places.Count)];
                    while(newPlace == place && j++ < 10000);
                }
                else
                {
                    newPlace = Vector2Int.zero;
                }
                
                var temp = _elements[newPlace.x, newPlace.y];
                _elements[newPlace.x, newPlace.y] = redoElement;
                redoElement.ChangePosition(newPlace);
                redoElement = temp;

                place = newPlace;
                
                if (places.Count == 0)
                {
                    break;
                }
                places.Remove(newPlace);
            }
        }
        
        ~Grid()
        {
            Dissolve();
        }

        public void Dissolve()
        {
            foreach (var element in _elements) 
                element.View?.Dissolve();
        }
    }
}