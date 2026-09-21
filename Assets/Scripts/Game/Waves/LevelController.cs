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
        [SerializeField] private TotalCrystalsView _totalCrystalsView;

        private GameCurrency _gameCurrency;
        private GlobalCurrency _globalCurrency;

        private void Awake()
        {
            _gameCurrency = new GameCurrency();
            _globalCurrency = new GlobalCurrency();
            
            _levelView.Init(_gameCurrency);
            _totalCrystalsView.Init(_globalCurrency);
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

        public void StartLevel(LevelData levelData)
        {
            _gameCurrency.Reset();
            _resultPanel.Hide();
            _levelView.Show();
            
            GameEvents.NotifyLevelStarted(levelData);
            _waveSystem.StartLevel(levelData);
        }
        
        public void HandleVictory()
        {
            _levelView.Hide();
            _resultPanel.ShowVictory(_waveSystem.CrystalEarnedThisLevel);
            _gameCurrency.Reset();
        }
        public void HandleDefeat()
        {
            _waveSystem.StopLevel();
            _levelView.Hide();
            _resultPanel.ShowDefeat(_waveSystem.CrystalEarnedThisLevel);
            _gameCurrency.Reset();
        }

        private void OnDestroy()
        {
            _gameCurrency.Dispose();
            _globalCurrency.Dispose();
        }
    }
}