using Game.Currency;
using Game.Events;
using Game.Waves;
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
            EventBus.Subscribe<HealthChange>(HandleTowerHealthChanged);
            EventBus.Subscribe<WaveChange>(HandleWaveProgress);
            EventBus.Subscribe<LevelData>(HandleLevelStarted);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<HealthChange>(HandleTowerHealthChanged);
            EventBus.Unsubscribe<WaveChange>(HandleWaveProgress);
            EventBus.Unsubscribe<LevelData>(HandleLevelStarted);
        }
        
        public void Show() => _rootPanel.SetActive(true);
        public void Hide() => _rootPanel.SetActive(false);

        private void RefreshCurrentValues()
        {
            HandleGoldChanged(_gameCurrency.Gold);
            HandleCrystalChanged(_gameCurrency.CrystalsThisLevel);
        }

        private void HandleLevelStarted(LevelData level)
        {
            _levelNameText.text = level.LevelName;
            _waveProgressText.text = $"Волна 0/{level.Waves.Count}";
        }

        private void HandleWaveProgress(WaveChange wave)
        {
            _waveProgressText.text = $"Волна {wave.Current}/{wave.Total}";
        }

        private void HandleTowerHealthChanged(HealthChange health)
        {
            _towerHealthText.text = $"{Mathf.CeilToInt(health.Current)}/{Mathf.CeilToInt(health.Max)}";
            _towerHealthBar.fillAmount = health.Current / health.Max;
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