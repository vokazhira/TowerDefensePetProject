using Game;
using ScriptableObjectData.LevelSO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionPanel : MonoBehaviour
    {
        [SerializeField] private LevelCatalogSO _catalog;
        [SerializeField] private TMP_Text _levelNameText;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _playButton;

        private int _currentIndex;
        
        private void Start()
        {
            if (_catalog == null || _catalog.Count == 0)
            {
                _playButton.interactable = false;
                return;
            }
            
            _currentIndex = Mathf.Max(0, _catalog.IndexOf(GameSession.Instance.SelectedLevel));

            _prevButton.onClick.AddListener(ShowPrevious);
            _nextButton.onClick.AddListener(ShowNext);
            _playButton.onClick.AddListener(PlaySelected);

            Refresh();
        }
        
        private void ShowNext()
        {
            if (_currentIndex >= _catalog.Count - 1) return;

            _currentIndex++;
            Refresh();
        }

        private void ShowPrevious()
        {
            if (_currentIndex <= 0) return;

            _currentIndex--;
            Refresh();
        }

        private void PlaySelected()
        {
            if (!IsUnlocked(_currentIndex)) return;

            GameSession.Instance.OpenLevel(_catalog.Get(_currentIndex));
        }

        private void Refresh()
        {
            LevelData level = _catalog.Get(_currentIndex);
            GameSession.Instance.SelectLevel(level);

            _levelNameText.text = level.LevelName;

            _prevButton.interactable = _currentIndex > 0;
            _nextButton.interactable = _currentIndex < _catalog.Count - 1;
            _playButton.interactable = IsUnlocked(_currentIndex);
        }
        
        private bool IsUnlocked(int index)
        {
            if (index == 0) return true;

            LevelData previousLevel = _catalog.Get(index - 1);
            return GameSession.Instance.IsLevelCompleted(previousLevel);
        }

        private void OnDestroy()
        {
            _prevButton.onClick.RemoveListener(ShowPrevious);
            _nextButton.onClick.RemoveListener(ShowNext);
            _playButton.onClick.RemoveListener(PlaySelected);
        }
    }
}