using Game.Events;
using UI.Popups;

namespace Towers.Combat
{
    public class DamageEventHitEffect : IHitEffect
    {
        public void Apply(HitContext context)
        {
            if (context.Target == null) return;
            
            EventBus.Invoke(new EnemyDamaged(
                context.Target.transform.position,
                context.Damage.Amount,
                context.Damage.IsCritical));
        }
    }
}