using Game.Currency;
using Game.Events;
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
            
            EventBus.Invoke<LevelData>(levelData);
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