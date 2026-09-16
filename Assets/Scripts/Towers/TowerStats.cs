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
    }
}