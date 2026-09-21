using Game.Currency;
using Game.Events.Observer;
using ScriptableObjectData.LevelSO;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TowerHealth _towerHealth;
        
        [SerializeField] private TMP_Text _levelNameText;
        [SerializeField] private TMP_Text _waveProgressText;
        [SerializeField] private TMP_Text _towerHealthText;
        [SerializeField] private Image _towerHealthBar;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _crystalText;
        
        private GameCurrency _gameCurrency;

        public void Init(GameCurrency gameCurrency)
        {
            _gameCurrency = gameCurrency;
            _gameCurrency.OnGoldChanged += HandleGoldChanged;
            _gameCurrency.OnCrystalsChanged += HandleCrystalChanged;
            
            RefreshCurrentValues();
        }
        
        private void OnEnable()
        {
            GameEvents.OnLevelStarted += HandleLevelStarted;
            GameEvents.OnWaveProgressChanged += HandleWaveProgress;
            _towerHealth.OnHealthChanged += HandleTowerHealthChanged;
        }

        private void OnDisable()
        {
            GameEvents.OnLevelStarted -= HandleLevelStarted;
            GameEvents.OnWaveProgressChanged -= HandleWaveProgress;
            _towerHealth.OnHealthChanged -= HandleTowerHealthChanged;
        }
        
        public void Show() => _rootPanel.SetActive(true);
        public void Hide() => _rootPanel.SetActive(false);

        private void RefreshCurrentValues()
        {
            HandleGoldChanged(_gameCurrency.Gold);
            HandleCrystalChanged(_gameCurrency.CrystalsThisLevel);
            
            HandleTowerHealthChanged(_towerHealth.CurrentHealth, _towerHealth.MaxHealth);
        }

        private void HandleLevelStarted(LevelData level)
        {
            _levelNameText.text = level.LevelName;
            _waveProgressText.text = $"Волна 0/{level.Waves.Count}";
            
            HandleTowerHealthChanged(_towerHealth.CurrentHealth, _towerHealth.MaxHealth);
        }

        private void HandleWaveProgress(int currentWave, int totalWaves)
        {
            _waveProgressText.text = $"Волна {currentWave}/{totalWaves}";
        }

        private void HandleTowerHealthChanged(float current, float max)
        {
            _towerHealthText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
            _towerHealthBar.fillAmount = current / max;
        }
        
        private void HandleGoldChanged(int gold) => _goldText.text = gold.ToString();
        private void HandleCrystalChanged(int crystal) => _crystalText.text = crystal.ToString();

        private void OnDestroy()
        {
            _gameCurrency.OnGoldChanged -= HandleGoldChanged;
            _gameCurrency.OnCrystalsChanged -= HandleCrystalChanged;
        }
    }
}