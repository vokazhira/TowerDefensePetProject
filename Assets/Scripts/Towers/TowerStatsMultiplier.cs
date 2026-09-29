using System;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class TowerStatsMultiplier
    {
        [Header("Fight stats")]
        public float Damage;
        public float AttackSpeed;
        public float Range;
        public float MultishotChance;
        public float CritChance;
        public float CritMultiplier;
        
        [Header("Defense Stats")]
        public float MaxHealth;
        public float HealthRegeneration;
        public float Defense;
        public float VampirismChance;
        public float VampirismMultiplier;
        
        [Header("Resource stats")]
        public int WaveGoldReward = 1;
        public int WaveCrystalReward = 1;
        public int GoldPerKillBonus = 1;
        public float FreeUpgradeChance = 0.25f;
        
        public float Get(TowerStatType statType)
        {
            return statType switch
            {
                TowerStatType.Damage => Damage,
                TowerStatType.AttackSpeed => AttackSpeed,
                TowerStatType.Range => Range,
                TowerStatType.MultishotChance => MultishotChance,
                TowerStatType.CritChance => CritChance,
                TowerStatType.CritMultiplier => CritMultiplier,
                TowerStatType.MaxHealth => MaxHealth,
                TowerStatType.HealthRegeneration => HealthRegeneration,
                TowerStatType.Defense => Defense,
                TowerStatType.VampirismChance => VampirismChance,
                TowerStatType.VampirismMultiplier => VampirismMultiplier,
                TowerStatType.WaveGoldReward => WaveGoldReward,
                TowerStatType.WaveCrystalReward => WaveCrystalReward,
                TowerStatType.GoldPerKillBonus => GoldPerKillBonus,
                TowerStatType.FreeUpgradeChance => FreeUpgradeChance,
                _ => 0f
            };
        }
    }
}