using Enemies;
using Lean.Pool;
using Towers.Combat;
using UnityEngine;

namespace Towers.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Enemy _target; 
        protected DamageInfo _damage;
        private IHitHandler _hitHandler;

        public virtual void Init(Enemy target, DamageInfo damage, IHitHandler hitHandler)
        {
            _target = target;
            _damage = damage;
            _hitHandler = hitHandler;
        }

        protected virtual void HitTarget()
        {
            ApplyHit(_target, _damage);
            Despawn();
        }

        protected void ApplyHit(Enemy enemy, DamageInfo damage)
        {
            if (IsAlive(enemy))
            {
                _hitHandler.HandleHit(enemy, damage);
            }
        }

        protected void Despawn()
        {
            LeanPool.Despawn(gameObject);
        }
        
        protected static bool IsAlive(Enemy enemy)
        {
            return enemy != null && enemy.gameObject.activeInHierarchy && !enemy.Health.IsDead;
        }

        protected virtual void OnDisable()
        {
            _target = null;
            _hitHandler = null;
        }
    }
}