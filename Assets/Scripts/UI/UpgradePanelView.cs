using Game;
using Game.Currency;
using Towers;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class UpgradePanelView : MonoBehaviour
    {
        [SerializeField] private UpgradeSystem _upgradeSystem;
        [SerializeField] private StatUpgradeButton[] _buttons;
        
        private GameCurrency _gameCurrency;

        public void Init(GameCurrency gameCurrency = null)
        {
            _gameCurrency = gameCurrency;
            
            _upgradeSystem.Init(gameCurrency);
            _upgradeSystem.OnUpgradeChanged += HandleUpgradeChanged;

            foreach (StatUpgradeButton button in _buttons)
            {
                button.Init(_upgradeSystem);
            }

            if (_gameCurrency != null)
            {
                _gameCurrency.OnGoldChanged += OnCurrencyChanged;
            }
            else
            {
                GameSession.Instance.OnTotalCrystalsChanged += OnCurrencyChanged;
            }
            
            Refresh();
        }

        private void OnCurrencyChanged(int _)
        {
            Refresh();
        }
        
        private void HandleUpgradeChanged(TowerStatType statType)
        {
            Refresh();
        }
        
        private void Refresh()
        {
            foreach (StatUpgradeButton button in _buttons)
            {
                if (button != null)
                {
                    button.Refresh();
                }
            }
        }

        private void OnDestroy()
        {
            _upgradeSystem.OnUpgradeChanged -= HandleUpgradeChanged;

            if (_gameCurrency != null)
            {
                _gameCurrency.OnGoldChanged -= OnCurrencyChanged;
            }
            else if (GameSession.Instance != null)
            {
                GameSession.Instance.OnTotalCrystalsChanged -= OnCurrencyChanged;
            }
        }
    }
}