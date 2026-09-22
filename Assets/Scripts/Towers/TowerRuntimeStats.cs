using System;
using Game;
using ScriptableObjectData.TowerSO;
using UnityEngine;

namespace Towers
{
    public class TowerRuntimeStats : MonoBehaviour
    {
        public TowerStats Stats {get; private set;}
        public TowerDataSO TowerData {get; private set;}
        
        public event Action<TowerStatType> OnStatChange;
        
        public void Init(TowerDataSO towerData)
        {
            TowerData = towerData;
            Stats = TowerData.TowerStats.Copy();

            foreach (TowerStatType statType in Enum.GetValues(typeof(TowerStatType)))
            {
                int permanentLevel = GameSession.Instance.GetPermanentLevel(statType);

                for (int i = 0; i < permanentLevel; i++)
                {
                    AddMultiplier(statType);
                }
            }
        }

        public void UpgradeForCurrentLevel(TowerStatType statType)
        {
            AddMultiplier(statType);
            OnStatChange?.Invoke(statType);
        }

        public float GetValue(TowerStatType statType)
        {
            return statType switch
            {
                TowerStatType.Damage => Stats.Damage,
                TowerStatType.AttackSpeed => Stats.AttackSpeed,
                TowerStatType.Range => Stats.Range,
                TowerStatType.MultishotChance => Stats.MultishotChance,
                TowerStatType.CritChance => Stats.CritChance,
                TowerStatType.CritMultiplier => Stats.CritMultiplier,
                TowerStatType.MaxHealth => Stats.MaxHealth,
                TowerStatType.HealthRegeneration => Stats.HealthRegeneration,
                TowerStatType.Defense => Stats.Defense,
                TowerStatType.VampirismChance => Stats.VampirismChance,
                TowerStatType.VampirismMultiplier => Stats.VampirismMultiplier,
                TowerStatType.WaveGoldReward => Stats.WaveGoldReward,
                TowerStatType.WaveCrystalReward => Stats.WaveCrystalReward,
                TowerStatType.GoldPerKillBonus => Stats.GoldPerKillBonus,
                TowerStatType.FreeUpgradeChance => Stats.FreeUpgradeChance,
                _ => 0f
            };
        }
        
        private void AddMultiplier(TowerStatType statType)
        {
            TowerStatsMultiplier multiplier = TowerData.Multipliers;

            switch (statType)
            {
                case TowerStatType.Damage: Stats.Damage += multiplier.Damage; break;
                case TowerStatType.AttackSpeed: Stats.AttackSpeed += multiplier.AttackSpeed; break;
                case TowerStatType.Range: Stats.Range += multiplier.Range; break;
                case TowerStatType.MultishotChance: Stats.MultishotChance += multiplier.MultishotChance; break;
                case TowerStatType.CritChance: Stats.CritChance += multiplier.CritChance; break;
                case TowerStatType.CritMultiplier: Stats.CritMultiplier += multiplier.CritMultiplier; break;
                case TowerStatType.MaxHealth: Stats.MaxHealth += multiplier.MaxHealth; break;
                case TowerStatType.HealthRegeneration: Stats.HealthRegeneration += multiplier.HealthRegeneration; break;
                case TowerStatType.Defense: Stats.Defense += multiplier.Defense; break;
                case TowerStatType.VampirismChance: Stats.VampirismChance += multiplier.VampirismChance; break;
                case TowerStatType.VampirismMultiplier: Stats.VampirismMultiplier += multiplier.VampirismMultiplier; break;
                    
                case TowerStatType.WaveGoldReward: break;
                case TowerStatType.WaveCrystalReward: break;
                case TowerStatType.GoldPerKillBonus:  break;
                case TowerStatType.FreeUpgradeChance: break;
            }
        }
    }
}