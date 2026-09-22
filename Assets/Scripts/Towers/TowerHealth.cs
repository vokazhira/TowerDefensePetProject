using System;
using Game.Events.Observer;
using Interfaces.Damage;
using UnityEngine;

namespace Towers
{
    public class TowerHealth : MonoBehaviour, IDamageable
    {
        public float CurrentHealth { get; private set; }
        public float MaxHealth {get; private set;}
        
        private float _defense;
        private float _regeneration;
        
        public bool IsDead => CurrentHealth <= 0f;
        
        public event Action<float, float> OnHealthChanged;

        public void ConfigureAtLevelStart(TowerStats towerStats)
        {
            ApplyStats(towerStats);
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void ApplyStats(TowerStats towerStats)
        {
            MaxHealth = towerStats.MaxHealth;
            _defense = towerStats.Defense;
            _regeneration = towerStats.HealthRegeneration;
            
            CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        private void Update()
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + _regeneration * Time.deltaTime);
                OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }
            
            float finalDamage = Mathf.Max(0f, damage - _defense);
            if (finalDamage <= 0f) return;

            CurrentHealth = Mathf.Max(0f, CurrentHealth - finalDamage);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0f)
            {
                GameEvents.NotifyTowerDestroyed();
            }
        }
    }
}