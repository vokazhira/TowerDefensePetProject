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
            _resultTitleText.text = "Победа!";
            _crystalsEarnedText.text = $"+{crystalEarnedThisLevel} кристаллов";
            _rootPanel.SetActive(true);
        }

        public void ShowDefeat(int crystalEarnedThisLevel)
        {
            _resultTitleText.text = "Поражение";
            _crystalsEarnedText.text = $"+{crystalEarnedThisLevel} кристаллов";
            _rootPanel.SetActive(true);
        }
    }
}