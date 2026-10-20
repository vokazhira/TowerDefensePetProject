using Game.Currency;
using Game.Events;
using Interfaces;
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
        [SerializeField] private NextWaveButtonView _nextWaveButton;
        
        private bool _isFinished;
        
        private void OnEnable()
        {
            EventBus.Subscribe<AllWavesCompleted>(HandleVictory);
            EventBus.Subscribe<TowerDestroyed>(HandleDefeat);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<AllWavesCompleted>(HandleVictory);
            EventBus.Unsubscribe<TowerDestroyed>(HandleDefeat);
        }

        public void StartSelectedLevel(IResourceRewardSource rewards)
        {
            LevelData levelData = GameSession.Instance.SelectedLevel;

            if (levelData == null)
            {
                GameSession.Instance.ReturnToMenu();
                return;
            }
            
            _isFinished = false;
            _resultPanel.Hide();
            _levelView.Show();
            _nextWaveButton.Init(_waveSystem);
            
            EventBus.Invoke<LevelData>(levelData);
            _waveSystem.StartLevel(levelData, rewards);
        }

        private void HandleVictory()
        {
            if (_isFinished) return;

            GameSession.Instance.CompleteSelectedLevel();
            _resultPanel.ShowVictory(FinishLevel());
        }

        private void HandleDefeat()
        {
            if (_isFinished) return;

            _waveSystem.StopLevel();
            _resultPanel.ShowDefeat(FinishLevel());
        }
        
        private LevelResult FinishLevel()
        {
            _isFinished = true;
            _levelView.Hide();

            LevelResult result = new LevelResult(
                _waveSystem.CrystalEarnedThisLevel,
                _waveSystem.CompletedWaves,
                _waveSystem.TotalWaves);

            GameSession.Instance.AddCrystals(result.Crystals);
            return result;
        }
    }
}