using System;
using System.Collections.Generic;
using Game;
using Game.Chances;
using Game.Currency;
using ScriptableObjectData.TowerSO;
using Towers;
using UnityEngine;
using Upgrades.Prices;

namespace Upgrades
{
    public class UpgradeSystem : MonoBehaviour
    {
        [SerializeField] private UpgradeMode _mode;
        [SerializeField] private TowerRuntimeStats _towerRuntimeStats;
        [SerializeField] private TowerDataSO _towerData;
        
        public event Action<TowerStatType> OnUpgradeChanged;

        private UpgradePriceCatalog _priceCatalog = new UpgradePriceCatalog();
        private IChanceRoller _chanceRoller = new ChanceRoller();
        private Dictionary<TowerStatType, int> _battleLevels = new Dictionary<TowerStatType, int>();
        private GameCurrency _gameCurrency;
        
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
            return _priceCatalog.GetPrice(statType, GetBoughtCount(statType));
        }

        public bool CanBuy(TowerStatType statType)
        {
            if (IsMaxed(statType)) return false;
            
            int price = GetPrice(statType);
            
            return _mode == UpgradeMode.Permanent
                ? GameSession.Instance.TotalCrystals >= price
                : _gameCurrency.Gold >= price;
        }

        public bool IsMaxed(TowerStatType statType)
        {
            return TowerStatRules.IsMaxed(statType, GetCurrentValue(statType));
        }

        public void TryUpgrade(TowerStatType statType)
        {
            if (!CanBuy(statType)) return;
            
            int price = GetPrice(statType);

            if (_mode == UpgradeMode.Permanent)
            {
                if (!GameSession.Instance.TrySpendCrystals(price)) return;
                
                GameSession.Instance.IncreasePermanentLevel(statType);
            }
            else
            {
                bool isFree = RollFreeUpgrade();
                
                if (!isFree && !_gameCurrency.TrySpendGold(price)) return;
                
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
            
            return TowerStatRules.Format(statType, GetCurrentValue(statType));
        }

        public float GetCurrentValue(TowerStatType statType)
        {
            return _mode == UpgradeMode.Battle
                ? _towerRuntimeStats.GetValue(statType)
                : CalculatePermanentValue(statType);
        }

        private int GetBoughtCount(TowerStatType statType)
        {
            return _mode == UpgradeMode.Permanent
                ? GameSession.Instance.GetPermanentLevel(statType)
                : _battleLevels[statType];
        }

        private float CalculatePermanentValue(TowerStatType statType)
        {
            if (_towerData == null) return 0f;

            TowerStats preview = _towerData.TowerStats.Copy();
            preview.ClampToLimits();
            
            int level = GameSession.Instance.GetPermanentLevel(statType);

            for (int i = 0; i < level; i++)
            {
                preview.Upgrade(statType, _towerData.Multipliers);
            }

            return preview.Get(statType);
        }
        
        private bool RollFreeUpgrade()
        {
            return _chanceRoller.Roll(_towerRuntimeStats.Stats.FreeUpgradeChance);
        }
    }
}