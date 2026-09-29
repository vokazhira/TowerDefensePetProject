using Game.Chances;
using UnityEngine;

namespace Towers.Combat
{
    public class CritDamageModifier : IDamageModifier
    {
        private IChanceRoller _chanceRoller;

        public CritDamageModifier(IChanceRoller chanceRoller)
        {
            _chanceRoller = chanceRoller;
        }
        
        public DamageInfo Modify(DamageInfo damage, TowerStats stats)
        {
            if (!_chanceRoller.Roll(stats.CritChance)) return damage;
            
            float multiplier = Mathf.Max(1f, stats.CritMultiplier);
            return new DamageInfo(damage.Amount * multiplier, true);
        }
    }
}