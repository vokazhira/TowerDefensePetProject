using Game.Chances;
using Interfaces.Damage;

namespace Towers.Combat
{
    public class VampirismHitEffect : IHitEffect
    {
        private IHealable _healable;
        private IChanceRoller _chanceRoller;

        public VampirismHitEffect(IHealable healable, IChanceRoller chanceRoller)
        {
            _healable = healable;
            _chanceRoller = chanceRoller;
        }
        
        public void Apply(HitContext context)
        {
            if (context.Stats.VampirismMultiplier <= 0f) return;
            if (!_chanceRoller.Roll(context.Stats.VampirismChance)) return;

            float healAmount = context.Damage.Amount * context.Stats.VampirismMultiplier;
            _healable.Heal(healAmount);
        }
    }
}