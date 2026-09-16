using Enemies;
using Interfaces.Damage;
using Lean.Pool;
using UnityEngine;

namespace Towers.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Enemy _target; 
        protected float _damage;

        public virtual void Init(Enemy target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        protected virtual void HitTarget()
        {
            if (_target != null && !_target.Health.IsDead)
            {
                _target.Health.TakeDamage(_damage);
            }
            LeanPool.Despawn(this.gameObject);
        }
    }
}