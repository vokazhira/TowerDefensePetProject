using System;
using GameData.Observer;
using Lean.Pool;
using Towers;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private const string TowerContactTag = "TowerContact";
        
        private EnemyHealth _health;
        private EnemyMovement _movement;
        private EnemyStats _stats;
        
        private bool _isActive;
        private TowerHealth _cachedTowerHealth;
        
        public EnemyHealth Health => _health;
        public EnemyStats Stats => _stats;

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _movement = GetComponent<EnemyMovement>();
        }

        public void Init(EnemyStats stats, Transform towerTransform)
        {
            _stats = stats;
            _isActive = true;
            _cachedTowerHealth = null;
            
            _health.Init(_stats.MaxHealth, this);
            _movement.Init(towerTransform, stats.MoveSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive || _health.IsDead) return;
            if (!other.CompareTag(TowerContactTag)) return;

            if (_cachedTowerHealth == null)
            {
                _cachedTowerHealth = other.GetComponentInParent<TowerHealth>();
            }
            
            DealContact(_cachedTowerHealth);
        }

        private void DealContact(TowerHealth towerHealth)
        {
            if (towerHealth != null && !towerHealth.IsDead)
            {
                towerHealth.TakeDamage(_stats.ContactDamage);
            }
            
            _movement.Stop();
            _health.Die();
        }

        private void OnDisable()
        {
            _isActive = false;
            _stats = null;
            _movement.Stop();
        }
    }
}