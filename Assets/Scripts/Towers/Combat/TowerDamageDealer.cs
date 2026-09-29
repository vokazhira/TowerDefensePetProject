using System.Collections.Generic;
using Enemies;

namespace Towers.Combat
{
    public class TowerDamageDealer : IHitHandler
    {
        private ITowerStatsProvider _statsProvider;
        private List<IDamageModifier> _damageModifiers;
        private List<IHitEffect> _hitEffects;

        public TowerDamageDealer(
            ITowerStatsProvider statsProvider, 
            IEnumerable<IDamageModifier> damageModifiers, 
            IEnumerable<IHitEffect> hitEffects)
        {
            _statsProvider = statsProvider;
            _damageModifiers = new List<IDamageModifier>(damageModifiers);
            _hitEffects = new List<IHitEffect>(hitEffects);
        }

        public DamageInfo CreateDamage()
        {
            TowerStats stats = _statsProvider.Stats;
            DamageInfo damage = new DamageInfo(stats.Damage);

            foreach (IDamageModifier modifier in _damageModifiers)
            {
                damage = modifier.Modify(damage, stats);
            }
            
            return damage;
        }
        
        public void HandleHit(Enemy target, DamageInfo damage)
        {
            if (target == null || target.Health.IsDead) return;
            
            target.Health.TakeDamage(damage.Amount);
            
            HitContext context = new HitContext(target, damage, _statsProvider.Stats);

            foreach (IHitEffect effect in _hitEffects)
            {
                effect.Apply(context);
            }
        }
    }
}