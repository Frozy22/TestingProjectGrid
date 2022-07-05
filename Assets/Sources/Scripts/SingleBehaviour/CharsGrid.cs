using System.Collections;
using System.Collections.Generic;
using Sources.Core;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.SingleBehaviour
{
    public class CharsGrid : MonoBehaviour
    {
        [SerializeField] private RectTransform _bounds;
        [SerializeField] private RectTransform _prefab;
        [SerializeField] private TMP_InputField _widthInputField;
        [SerializeField] private TMP_InputField _heightInputField;
        [SerializeField] private MessageBox _messageBox;
        [SerializeField] private string _widthParseFailMessage = "Ширина: неправильный формат ввода";
        [SerializeField] private string _heightParseFailMessage = "Высота: неправильный формат ввода";
        [SerializeField] private string _gridNullMessage = "Перемешивание: сетка ещё не сгенерирована";
        [SerializeField] private float _elementMoveTime = 2f;
    
        private RectTransform[,] _grid;
        private int _xCount;
        private int _yCount;

        public void Clear()
        {
            if (_grid == null)
                return;
        
            for (int x = 0; x < _grid.GetLength(0); x++)
            for (int y = 0; y < _grid.GetLength(1); y++)
                Destroy(_grid[x, y].gameObject);
        }
    
        public void Generate()
        {
            //int xCount, yCount;
        
            if(int.TryParse(_widthInputField.text, out _xCount) == false)
            {
                _messageBox.Show(_widthParseFailMessage);
                return;
            }
        
            if(int.TryParse(_heightInputField.text, out _yCount) == false)
            {
                _messageBox.Show(_heightParseFailMessage);
                return;
            }
        
            Clear();
            _grid = GenerateGrid(_xCount, _yCount, _prefab);
        }

        public void Shuffle()
        {
            if(_grid == null)
            {
                _messageBox.Show(_gridNullMessage);
                return;
            }
            ShuffleGrid(_grid);
        }

        public void ShuffleGrid(RectTransform[,] grid)
        {
            RectTransform[,] prevGrid = new RectTransform[_xCount, _yCount];
        
            List<Vector2Int> places = new List<Vector2Int>();
            for (int x = 0; x < _xCount; x++)
            for (int y = 0; y < _yCount; y++)
            {
                places.Add(new Vector2Int(x, y));
                prevGrid[x, y] = grid[x, y];
            }

            float delta = CalculateDelta(_xCount, _yCount);
        
            for (int x = 0; x < _xCount; x++)
            for (int y = 0; y < _yCount; y++)
            {
                Vector2Int place = new Vector2Int(x, y);
                Vector2Int newPlace;
                int i = 0;
                do newPlace = places[Random.Range(0, places.Count)];
                while(newPlace == place && i++ < 10000);
            
                Vector2 newPos = PositionByIndex(_xCount, _yCount, newPlace.x, newPlace.y, delta);
            
                StartCoroutine(ElementMove(prevGrid[place.x,place.y], newPos));
                grid[newPlace.x, newPlace.y] = prevGrid[place.x,place.y];
                places.Remove(newPlace);
            }
        }

        public IEnumerator ElementMove(RectTransform element, Vector2 to)
        {
            Vector2 at = element.anchoredPosition;
            float timer = 0f;
            while (timer <= _elementMoveTime)
            {
                element.anchoredPosition = Vector2.Lerp(at, to, timer/_elementMoveTime);
                yield return null;
                timer += Time.deltaTime;
            }
            element.anchoredPosition = to;
        }

        public RectTransform[,] GenerateGrid(int xCount, int yCount, RectTransform prefab)
        {
            float delta = CalculateDelta(xCount, yCount);

            RectTransform[,] grid = new RectTransform[xCount, yCount];
            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    grid[x, y] = InstantiateElement(xCount, yCount, x, y, delta, prefab);
                }
            }

            return grid;
        }

        private float CalculateDelta(int xCount, int yCount)
        {
            float deltaX = _bounds.rect.width / xCount;
            float deltaY = _bounds.rect.height / yCount;
            return deltaX > deltaY ? deltaY : deltaX;
        }

        private Vector2 PositionByIndex(int xCount, int yCount, int xIndex, int yIndex, float delta) 
            => new Vector2(xIndex - xCount / 2f, yIndex - yCount / 2f) * delta;

        private const string symbols = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private RectTransform InstantiateElement(int xCount, int yCount, int xIndex, int yIndex, float delta, RectTransform prefab)
        {
            RectTransform instance = Instantiate(prefab, _bounds);
            instance.pivot = new Vector2(0, 0);
            instance.anchorMin = new Vector2(0.5f, 0.5f);
            instance.anchorMax = new Vector2(0.5f, 0.5f);
            instance.sizeDelta = new Vector2(delta, delta);
            instance.anchoredPosition = PositionByIndex(xCount, yCount, xIndex, yIndex, delta);
            instance.GetComponent<TMP_Text>().text = symbols[Random.Range(0, symbols.Length)].ToString();
            return instance;
        }
    }
}
