using Lean.Pool;
using UnityEngine;

namespace Towers.Projectiles
{
    public class ArrowProjectile : Projectile
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _arrivalDistance;

        private void Update()
        {
            if (_target == null || !_target.gameObject.activeInHierarchy)
            {
                LeanPool.Despawn(gameObject);
                return;
            }
            
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            transform.position += direction * (_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _target.transform.position) <= _arrivalDistance)
            {
                HitTarget();
            }
        }
    }
}