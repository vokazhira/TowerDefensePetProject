using Enemies.Abilities;
using Interfaces.Damage;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private EnemyHealth _health;
        private EnemyMovement _movement;
        private EnemyAbility[] _abilities;
        private EnemyStats _stats;
        
        private bool _isActive;
        private IDamageable  _cachedContactTarget;
        
        public EnemyHealth Health => _health;
        public EnemyMovement Movement => _movement;
        public EnemyStats Stats => _stats;

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _movement = GetComponent<EnemyMovement>();
            _abilities = GetComponents<EnemyAbility>();

            _health.Killed += HandleKilled;
        }

        public void Init(EnemyStats stats, EnemyContext context)
        {
            _stats = stats;
            _isActive = true;
            _cachedContactTarget = null;
            
            _health.Init(_stats.MaxHealth, this);
            _movement.Init(context.Tower, stats.MoveSpeed);

            foreach (EnemyAbility ability in _abilities)
            {
                ability.Init(this, context);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive || _health.IsDead) return;
            if (!other.CompareTag("TowerContact")) return;

            if (_cachedContactTarget == null)
            {
                _cachedContactTarget = other.GetComponentInParent<IDamageable>();
            }
            
            DealContact(_cachedContactTarget);
        }

        private void HandleKilled()
        {
            foreach (EnemyAbility ability in _abilities)
            {
                ability.OnKilled();
            }
        }

        private void DealContact(IDamageable target)
        {
            target?.TakeDamage(_stats.ContactDamage);

            _movement.Stop();
            _health.Die();
        }

        private void OnDisable()
        {
            _isActive = false;
            _stats = null;
            _movement.Stop();
        }

        private void OnDestroy()
        {
            if  (_health != null)
            {
                _health.Killed -= HandleKilled;
            }
        }
    }
}