using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public static class TowerStatRules
    {
        private static readonly Dictionary<TowerStatType, float> _maxValues = new Dictionary<TowerStatType, float>
        {
            { TowerStatType.MultishotChance, 100f },
            { TowerStatType.CritChance, 100f },
            { TowerStatType.VampirismChance, 100f },
            { TowerStatType.FreeUpgradeChance, 100f },
            { TowerStatType.Defense, 90f },
        };
        
        private static readonly HashSet<TowerStatType> _percentStats = new HashSet<TowerStatType>
        {
            TowerStatType.MultishotChance,
            TowerStatType.CritChance,
            TowerStatType.VampirismChance,
            TowerStatType.FreeUpgradeChance,
            TowerStatType.Defense,
        };

        public static bool TryGetMax(TowerStatType type, out float max)
        {
            return _maxValues.TryGetValue(type, out max);
        }
        
        public static float Clamp(TowerStatType statType, float value)
        {
            return TryGetMax(statType, out float max) ? Mathf.Clamp(value, 0f, max) : value;
        }
        
        public static bool IsMaxed(TowerStatType statType, float value)
        {
            return TryGetMax(statType, out float max) && value >= max;
        }
        
        public static bool IsPercent(TowerStatType statType) => _percentStats.Contains(statType);

        public static string Format(TowerStatType statType, float value)
        {
            return IsPercent(statType) ? $"{value:0.##}%" : $"{value:0.##}";
        }
    }
}