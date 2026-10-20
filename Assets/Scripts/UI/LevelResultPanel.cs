using Game;
using Game.Waves;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelResultPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TMP_Text _resultTitleText;
        [SerializeField] private TMP_Text _crystalsEarnedText;
        [SerializeField] private TMP_Text _wavesText;
        
        public void Hide() => _rootPanel.SetActive(false);

        public void ShowVictory(LevelResult result) => Show("ПОБЕДА!", result);
        public void ShowDefeat(LevelResult result) => Show("ПОРАЖЕНИЕ", result);

        public void ReturnToMenu() => GameSession.Instance.ReturnToMenu();

        private void Show(string title, LevelResult result)
        {
            _resultTitleText.text = title;
            _crystalsEarnedText.text = $"{result.Crystals}";
            _wavesText.text = $"Волн пройдено: \n{result.CompletedWaves}/{result.TotalWaves}";
            _rootPanel.SetActive(true);
        }
    }
}