using System;
using System.Collections.Generic;
using Game;
using Game.Currency;
using Game.States;
using ScriptableObjectData.TowerSO;
using Towers;
using UnityEngine;
using UnityEngine.UI;
using Upgrades.Prices;
using Random = UnityEngine.Random;

namespace Upgrades
{
    public class UpgradeSystem : MonoBehaviour
    {
        [SerializeField] private UpgradeMode _mode;
        [SerializeField] private TowerRuntimeStats _towerRuntimeStats;
        
        public event Action<TowerStatType> OnUpgradeChanged;

        private UpgradePriceCatalog _priceCatalog = new UpgradePriceCatalog();
        private GameCurrency _gameCurrency;
        private Dictionary<TowerStatType, int> _battleLevels = new Dictionary<TowerStatType, int>();

        public void Init(GameCurrency gameCurrency)
        {
            _gameCurrency = gameCurrency;

            foreach (TowerStatType statType  in Enum.GetValues(typeof(TowerStatType)))
            {
                _battleLevels[statType ] = 0;
            }
        }

        public int GetPrice(TowerStatType statType)
        {
            int boughtCount;
            
            if (_mode == UpgradeMode.Permanent)
            {
                boughtCount = GameSession.Instance.GetPermanentLevel(statType);
            }
            else
            {
                boughtCount = _battleLevels[statType];
            }
            
            return _priceCatalog.GetPrice(statType, boughtCount);
        }

        public bool CanBuy(TowerStatType statType)
        {
            int price = GetPrice(statType);

            if (_mode == UpgradeMode.Permanent)
            {
                return GameSession.Instance.TotalCrystals >= price;
            }
            else
            {
                return _gameCurrency.Gold >= price;
            }
        }

        public void TryUpgrade(TowerStatType statType)
        {
            int price = GetPrice(statType);

            if (_mode == UpgradeMode.Permanent)
            {
                if (!GameSession.Instance.TrySpendCrystals(price))
                {
                    return;
                }
                
                GameSession.Instance.IncreasePermanentLevel(statType);
            }
            else
            {
                if (!RollFreeUpgrade())
                {
                    _gameCurrency.TrySpendGold(price);
                }
                
                _battleLevels[statType]++;
                _towerRuntimeStats.UpgradeForCurrentLevel(statType);
            }
            
            OnUpgradeChanged?.Invoke(statType);
        }

        public string GetValueText(TowerStatType statType)
        {
            if (_mode == UpgradeMode.Permanent)
            {
                return $"Ур.: {GameSession.Instance.GetPermanentLevel(statType)}";
            }
            
            float value = _towerRuntimeStats.GetValue(statType);
            
            return statType == TowerStatType.FreeUpgradeChance
                ? $"{value:0.##}%"
                : $"{value:0.##}";
        }
        
        private bool RollFreeUpgrade()
        {
            float chancePercent = _towerRuntimeStats.Stats.FreeUpgradeChance;
            return chancePercent > 0f && Random.Range(0f, 100f) < chancePercent;
        }
    }
}