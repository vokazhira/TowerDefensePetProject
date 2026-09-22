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
        
        //TODO: доделать ресурс статы
    }
}