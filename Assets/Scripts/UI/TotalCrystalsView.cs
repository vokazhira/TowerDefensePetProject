using Game.Currency;
using Game.Events.Observer;
using ScriptableObjectData.LevelSO;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TotalCrystalsView : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TMP_Text _totalCrystalsText;
        
        private GlobalCurrency _globalCurrency;

        public void Init(GlobalCurrency globalCurrency)
        {
            _globalCurrency = globalCurrency;
            _globalCurrency.OnCrystalsChanged += HandleCrystalsChanged;
            
            Refresh();
            Show();
        }

        private void OnEnable()
        {
            GameEvents.OnLevelStarted += HandleLevelStarted;
            GameEvents.OnAllWavesCompleted += HandleLevelEnded;
            GameEvents.OnTowerDestroyed += HandleLevelEnded;
        }

        private void OnDisable()
        {
            GameEvents.OnLevelStarted -= HandleLevelStarted;
            GameEvents.OnAllWavesCompleted -= HandleLevelEnded;
            GameEvents.OnTowerDestroyed -= HandleLevelEnded;
        }

        private void HandleLevelStarted(LevelData level) => Hide();
        private void HandleLevelEnded()
        {
            Refresh();
            Show();
        }

        private void HandleCrystalsChanged(int total) => _totalCrystalsText.text = total.ToString();
        
        private void Refresh() => HandleCrystalsChanged(_globalCurrency.Crystals);
        private void Show() => _rootPanel.SetActive(true);
        private void Hide() => _rootPanel.SetActive(false);
        
        private void OnDestroy() => _globalCurrency.OnCrystalsChanged -= HandleCrystalsChanged;
    }
}