using System;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class TowerStats
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
        public int WaveGoldReward;
        public int WaveCrystalReward;
        public int GoldPerKillBonus;
        public float FreeUpgradeChance;

        public TowerStats Copy()
        {
            return new TowerStats()
            {
                Damage = Damage,
                AttackSpeed = AttackSpeed,
                Range = Range,
                MultishotChance = MultishotChance,
                CritChance = CritChance,
                CritMultiplier = CritMultiplier,

                MaxHealth = MaxHealth,
                HealthRegeneration = HealthRegeneration,
                Defense = Defense,
                VampirismChance = VampirismChance,
                VampirismMultiplier = VampirismMultiplier,

                WaveGoldReward = WaveGoldReward,
                WaveCrystalReward = WaveCrystalReward,
                GoldPerKillBonus = GoldPerKillBonus,
                FreeUpgradeChance = FreeUpgradeChance,
            };
        }

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
        
        public void Set(TowerStatType statType, float value)
        {
            switch (statType)
            {
                case TowerStatType.Damage: Damage = value; break;
                case TowerStatType.AttackSpeed: AttackSpeed = value; break;
                case TowerStatType.Range: Range = value; break;
                case TowerStatType.MultishotChance: MultishotChance = value; break;
                case TowerStatType.CritChance: CritChance = value; break;
                case TowerStatType.CritMultiplier: CritMultiplier = value; break;
                case TowerStatType.MaxHealth: MaxHealth = value; break;
                case TowerStatType.HealthRegeneration: HealthRegeneration = value; break;
                case TowerStatType.Defense: Defense = value; break;
                case TowerStatType.VampirismChance: VampirismChance = value; break;
                case TowerStatType.VampirismMultiplier: VampirismMultiplier = value; break;
                case TowerStatType.WaveGoldReward: WaveGoldReward = Mathf.RoundToInt(value); break;
                case TowerStatType.WaveCrystalReward: WaveCrystalReward = Mathf.RoundToInt(value); break;
                case TowerStatType.GoldPerKillBonus: GoldPerKillBonus = Mathf.RoundToInt(value); break;
                case TowerStatType.FreeUpgradeChance: FreeUpgradeChance = value; break;
            }
        }

        public void Upgrade(TowerStatType statType, TowerStatsMultiplier multipliers)
        {
            float upgraded = Get(statType) + multipliers.Get(statType);
            Set(statType, TowerStatRules.Clamp(statType, upgraded));
        }
        
        public void ClampToLimits()
        {
            foreach (TowerStatType statType in Enum.GetValues(typeof(TowerStatType)))
            {
                Set(statType, TowerStatRules.Clamp(statType, Get(statType)));
            }
        }
    }
}