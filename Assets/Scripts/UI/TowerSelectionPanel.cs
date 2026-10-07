using Game;
using ScriptableObjectData.TowerSO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerSelectionPanel : MonoBehaviour
    {
        [SerializeField] private TowerCatalogSO _catalog;
        [SerializeField] private Image _towerImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _prevButton;

        private int _currentIndex;

        private void Start()
        {
            if (_catalog == null || _catalog.Count == 0)
            {
                return;
            }
            
            _currentIndex = Mathf.Max(0, _catalog.IndexOf(GameSession.Instance.SelectedTower));
            
            _prevButton.onClick.AddListener(ShowPrevious);
            _nextButton.onClick.AddListener(ShowNext);
            
            SelectCurrent();
        }
        
        private void ShowNext()
        {
            _currentIndex++;

            if (_currentIndex >= _catalog.Count)
            {
                _currentIndex = 0;
            }

            SelectCurrent();
        }

        private void ShowPrevious()
        {
            _currentIndex--;

            if (_currentIndex < 0)
            {
                _currentIndex = _catalog.Count - 1;
            }

            SelectCurrent();
        }

        private void SelectCurrent()
        {
            TowerDataSO tower = _catalog.Get(_currentIndex);
            GameSession.Instance.SelectTower(tower);

            _towerImage.sprite = tower.Sprite;
            _towerImage.enabled = tower.Sprite != null;
            _nameText.text = tower.TowerName;
            _descriptionText.text = tower.Description;
        }

        private void OnDestroy()
        {
            _prevButton.onClick.RemoveListener(ShowPrevious);
            _nextButton.onClick.RemoveListener(ShowNext);
        }
    }
}