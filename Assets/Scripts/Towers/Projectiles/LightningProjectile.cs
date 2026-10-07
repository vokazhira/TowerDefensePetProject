using System.Collections.Generic;
using Enemies;
using Towers.Combat;
using UnityEngine;

namespace Towers.Projectiles
{
    public class LightningProjectile : HomingProjectile
    {
        [SerializeField] private int _maxHits = 4;
        [SerializeField, Range(0f, 1f)] private float _damageReductionBounce = 0.35f;
        [SerializeField] private float _bounceRadius = 2f;
        [SerializeField] private LayerMask _enemyLayer;

        private HashSet<Enemy> _hitEnemies = new HashSet<Enemy>();
        private int _hitsCount;

        public override void Init(Enemy target, DamageInfo damageInfo, IHitHandler hitHandler)
        {
            base.Init(target, damageInfo, hitHandler);
            _hitsCount = 0;
            _hitEnemies.Clear();
        }

        protected override void HitTarget()
        {
            _hitEnemies.Add(_target);
            ApplyHit(_target, _damage);
            _hitsCount++;

            if (_hitsCount >= _maxHits)
            {
                Despawn();
                return;
            }

            Enemy nextTarget = FindNearestEnemy();

            if (nextTarget == null)
            {
                Despawn();
                return;
            }
            
            _target = nextTarget;
            _damage = new DamageInfo(_damage.Amount * (1f - _damageReductionBounce));
        }

        private Enemy FindNearestEnemy()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _bounceRadius, _enemyLayer);
            
            Enemy nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                if (!collider.TryGetComponent(out Enemy enemy)) continue;
                if (!IsAlive(enemy) || _hitEnemies.Contains(enemy)) continue;
                
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = enemy;
                }
            }

            return nearest;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _hitEnemies.Clear();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _bounceRadius);
        }
    }
}