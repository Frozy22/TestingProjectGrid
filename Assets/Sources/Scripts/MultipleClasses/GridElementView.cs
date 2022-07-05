using System.Collections;
using TMPro;
using UnityEngine;

namespace Sources.MultipleClasses
{
    [RequireComponent(typeof(RectTransform))]
    public class GridElementView : MonoBehaviour, IGridElementView
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _moveTime = 2f;
        private Grid _grid;
        private RectTransform _bounds;
        private float _delta;
        private RectTransform _rectTransform;
        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(Grid grid, RectTransform bounds)
        {
            _grid = grid;
            _bounds = bounds;

            var rect = _bounds.rect;
            float deltaX = rect.width / _grid.CountX;
            float deltaY = rect.height / _grid.CountY;
            _delta = deltaX > deltaY ? deltaY : deltaX;
            
            _rectTransform.pivot = new Vector2(0, 0);
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.sizeDelta = new Vector2(_delta, _delta);
        }
        
        private Vector2 PositionByIndices(Vector2Int indices) 
            => new Vector2(indices.x - _grid.CountX / 2f, indices.y - _grid.CountY / 2f) * _delta;

        public void ValueChanged(char value)
        {
            _label.text = value.ToString();
        }
        
        public void SetPosition(Vector2Int to)
        {
            _rectTransform.anchoredPosition = PositionByIndices(to);
        }

        public void ChangePosition(Vector2Int to)
        {
            if(_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            Vector2 position = PositionByIndices(to);
            _moveCoroutine = StartCoroutine(ElementMove(position));
        }
        
        public IEnumerator ElementMove(Vector2 to)
        {
            Vector2 at = _rectTransform.anchoredPosition;
            float timer = 0f;
            while (timer <= _moveTime)
            {
                _rectTransform.anchoredPosition = Vector2.Lerp(at, to, timer/_moveTime);
                yield return null;
                timer += Time.deltaTime;
            }
            _rectTransform.anchoredPosition = to;
            _moveCoroutine = null;
        }


        public void Dissolve()
        {
            Destroy(gameObject);
        }
    }
}