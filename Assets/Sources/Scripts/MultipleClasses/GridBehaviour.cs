using Sources.Core;
using TMPro;
using UnityEngine;
namespace Sources.MultipleClasses
{
    public class GridBehaviour : MonoBehaviour
    {
        [SerializeField] private GridElementViewFactory _viewFactory;
        [SerializeField] private TMP_InputField _widthInputField;
        [SerializeField] private TMP_InputField _heightInputField;
        [SerializeField] private MessageBox _messageBox;
        [SerializeField] private string _widthParseFailMessage = "Ширина: неправильный формат ввода";
        [SerializeField] private string _heightParseFailMessage = "Высота: неправильный формат ввода";
        [SerializeField] private string _gridNullMessage = "Перемешивание: сетка ещё не сгенерирована";

        private Grid _grid;
        
        public void GenerateGrid()
        {
            int countX, countY;
            
            if(int.TryParse(_widthInputField.text, out countX) == false)
            {
                _messageBox.Show(_widthParseFailMessage);
                return;
            }
        
            if(int.TryParse(_heightInputField.text, out countY) == false)
            {
                _messageBox.Show(_heightParseFailMessage);
                return;
            }
            
            _grid?.Dissolve();

            _grid = new Grid(countX, countY, _viewFactory);
        }

        public void ShuffleGrid()
        {
            if(_grid == null)
            {
                _messageBox.Show(_gridNullMessage);
                return;
            }
            _grid.ShuffleElements();
        }
    }
}