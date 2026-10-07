using Enemies;
using UnityEngine;

namespace Towers.Projectiles
{
    public class BombProjectile : HomingProjectile
    {
        [SerializeField] private float _explosionRadius = 1.5f;
        [SerializeField] private LayerMask _enemyLayer;

        protected override void HitTarget()
        {
            Explode();
        }

        private void Explode()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayer);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy enemy))
                {
                    ApplyHit(enemy, _damage);
                }
            }

            Despawn();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, _explosionRadius);
        }
    }
}