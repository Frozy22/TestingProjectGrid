using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Core
{
    public class MessageBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Button _applyButton;

        private void Awake()
        {
            _applyButton.onClick.AddListener(Close);
        }

        public void Show(string message)
        {
            _label.text = message;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}