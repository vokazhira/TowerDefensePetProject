using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelResultPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TMP_Text _resultTitleText;
        [SerializeField] private TMP_Text _crystalsEarnedText;
        
        public void Hide() => _rootPanel.SetActive(false);

        public void ShowVictory(int crystalEarnedThisLevel)
        {
            Show("ПОБЕДА!", crystalEarnedThisLevel);
        }

        public void ShowDefeat(int crystalEarnedThisLevel)
        {
            Show("ПОРАЖЕНИЕ", crystalEarnedThisLevel);
        }

        public void ReturnToMenu()
        {
            GameSession.Instance.ReturnToMenu();
        }

        private void Show(string title, int crystals)
        {
            _resultTitleText.text = title;
            _crystalsEarnedText.text = $"+{crystals} кристаллов";
            _rootPanel.SetActive(true);
        }
    }
}