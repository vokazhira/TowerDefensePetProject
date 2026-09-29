using System;
using Game;
using Interfaces;
using ScriptableObjectData.TowerSO;
using UnityEngine;

namespace Towers
{
    public class TowerRuntimeStats : MonoBehaviour, IResourceRewardSource, ITowerStatsProvider
    {
        public TowerStats Stats {get; private set;}
        public TowerDataSO TowerData {get; private set;}
        
        public int WaveGoldReward => Stats.WaveGoldReward;
        public int WaveCrystalReward => Stats.WaveCrystalReward;
        public int GoldPerKillBonus => Stats.GoldPerKillBonus;
        
        public event Action<TowerStatType> OnStatChange;
        
        public void Init(TowerDataSO towerData)
        {
            TowerData = towerData;
            Stats = TowerData.TowerStats.Copy();
            Stats.ClampToLimits();

            ApplyPermanentUpgrades();
        }

        public void UpgradeForCurrentLevel(TowerStatType statType)
        {
            Stats.Upgrade(statType, TowerData.Multipliers);
            OnStatChange?.Invoke(statType);
        }

        public float GetValue(TowerStatType statType) => Stats.Get(statType);
        public bool IsMaxed(TowerStatType statType) => TowerStatRules.IsMaxed(statType, Stats.Get(statType));

        private void ApplyPermanentUpgrades()
        {
            foreach (TowerStatType statType in Enum.GetValues(typeof(TowerStatType)))
            {
                int permanentLevel = GameSession.Instance.GetPermanentLevel(statType);

                for (int i = 0; i < permanentLevel; i++)
                {
                    Stats.Upgrade(statType, TowerData.Multipliers);
                }
            }
        }
    }
}