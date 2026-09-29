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
            if (_target != null && !_target.Health.IsDead)
            {
                _hitHandler?.HandleHit(_target, _damage);
            }
            LeanPool.Despawn(this.gameObject);
        }

        protected virtual void OnDisable()
        {
            _target = null;
            _hitHandler = null;
        }
    }
}