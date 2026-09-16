using GameData.Observer;
using Interfaces.Damage;
using Lean.Pool;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        private float _maxHealth;
        private Enemy _owner;
        private bool _isDead;
        
        public float CurrentHealth {get; private set;}
        public float MaxHealth => _maxHealth;
        public bool IsDead => _isDead || CurrentHealth <= 0f;

        public void Init(float maxHealth, Enemy owner)
        {
            _maxHealth = maxHealth;
            CurrentHealth = _maxHealth;
            _owner = owner;
            _isDead = false;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, _maxHealth);

            if (CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            if (_isDead) return;
            _isDead = true;
            CurrentHealth = 0f;

            if (_owner != null && _owner.Stats != null)
            {
                GameEvents.NotifyGoldEarned(_owner.Stats.GoldReward);
                GameEvents.NotifyEnemyDied(_owner);
            }
            
            LeanPool.Despawn(this.gameObject);
        }
        
        private void OnDisable()
        {
            _isDead = false;
            _owner = null;
        }
    }
}