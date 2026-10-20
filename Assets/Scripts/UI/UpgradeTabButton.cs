using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeTabButton : MonoBehaviour 
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _content;
        [SerializeField] private Image _background;
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _normalColor = Color.gray;

        public event Action<UpgradeTabButton> Clicked;

        private void Awake() => _button.onClick.AddListener(OnClick);
        private void OnDestroy() => _button.onClick.RemoveListener(OnClick);

        public void SetSelected(bool isSelected)
        {
            _content.SetActive(isSelected);
            _background.color = isSelected ? _selectedColor : _normalColor;
        }

        private void OnClick() => Clicked?.Invoke(this);
    }
}