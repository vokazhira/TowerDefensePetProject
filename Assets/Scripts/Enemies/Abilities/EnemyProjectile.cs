using Interfaces.Damage;
using Lean.Pool;
using UnityEngine;

namespace Enemies.Abilities
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 6f;
        [SerializeField] private float _arrivalDistance = 0.3f;

        private Transform _target;
        private IDamageable _damageable;
        private float _damage;

        public void Init(Transform target, IDamageable damageable, float damage)
        {
            _target = target;
            _damageable = damageable;
            _damage = damage;
        }

        private void Update()
        {
            if (_target == null)
            {
                LeanPool.Despawn(gameObject);
                return;
            }

            Vector3 targetPosition = _target.position;
            transform.right = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) <= _arrivalDistance)
            {
                _damageable?.TakeDamage(_damage);
                LeanPool.Despawn(gameObject);
            }
        }

        private void OnDisable()
        {
            _target = null;
            _damageable = null;
        }
    }
}