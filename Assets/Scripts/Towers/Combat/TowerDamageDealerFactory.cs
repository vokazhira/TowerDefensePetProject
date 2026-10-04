using System.Collections.Generic;
using Game.Chances;
using Interfaces.Damage;

namespace Towers.Combat
{
    public class TowerDamageDealerFactory
    {
        private IChanceRoller _chanceRoller;

        //TODO: для разных видов башен сделать через SO
        
        public TowerDamageDealerFactory(IChanceRoller chanceRoller)
        {
            _chanceRoller = chanceRoller;
        }

        public TowerDamageDealer Create(ITowerStatsProvider statsProvider, IHealable healable)
        {
            List<IDamageModifier> modifiers = new List<IDamageModifier>
            {
                new CritDamageModifier(_chanceRoller)
            };

            List<IHitEffect> effects = new List<IHitEffect>
            {
                new VampirismHitEffect(healable, _chanceRoller),
                new DamageEventHitEffect()
            };

            return new TowerDamageDealer(statsProvider, modifiers, effects);
        }
    }
}