using Game.Currency;
using Game.Events.Observer;
using ScriptableObjectData.LevelSO;
using Towers;
using UI;
using UnityEngine;

namespace Game.Waves
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private WaveSystem _waveSystem;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private LevelResultPanel _resultPanel;

        private GameCurrency _gameCurrency;

        public void Init(GameCurrency currency)
        {
            _gameCurrency = currency;
        }
        
        private void OnEnable()
        {
            GameEvents.OnAllWavesCompleted += HandleVictory;
            GameEvents.OnTowerDestroyed += HandleDefeat;
        }

        private void OnDisable()
        {
            GameEvents.OnAllWavesCompleted -= HandleVictory;
            GameEvents.OnTowerDestroyed -= HandleDefeat;
        }

        public void StartSelectedLevel()
        {
            LevelData levelData = GameSession.Instance.SelectedLevel;

            if (levelData == null)
            {
                GameSession.Instance.ReturnToMenu();
                return;
            }
            
            _resultPanel.Hide();
            _levelView.Show();
            
            GameEvents.NotifyLevelStarted(levelData);
            _waveSystem.StartLevel(levelData);
        }
        
        public void HandleVictory()
        {
            _levelView.Hide();
            _resultPanel.ShowVictory(_waveSystem.CrystalEarnedThisLevel);
        }
        public void HandleDefeat()
        {
            _waveSystem.StopLevel();
            _levelView.Hide();
            _resultPanel.ShowDefeat(_waveSystem.CrystalEarnedThisLevel);
        }
    }
}