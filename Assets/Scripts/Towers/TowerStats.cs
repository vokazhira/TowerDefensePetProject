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
            
            //TODO: сделать с ICloneable
        }
    }
}