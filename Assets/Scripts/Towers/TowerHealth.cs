using System;
using Game.Events;
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

        public void ConfigureAtLevelStart(TowerStats towerStats)
        {
            ApplyStats(towerStats);
            CurrentHealth = MaxHealth;
            EventBus.Invoke(new HealthChange(CurrentHealth, MaxHealth));
        }

        public void ApplyStats(TowerStats towerStats)
        {
            MaxHealth = towerStats.MaxHealth;
            _defense = towerStats.Defense;
            _regeneration = towerStats.HealthRegeneration;
            
            CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
            EventBus.Invoke(new HealthChange(CurrentHealth, MaxHealth));
        }

        private void Update()
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + _regeneration * Time.deltaTime);
                EventBus.Invoke(new HealthChange(CurrentHealth, MaxHealth));
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
            EventBus.Invoke(new HealthChange(CurrentHealth, MaxHealth));

            if (CurrentHealth <= 0f)
            {
                EventBus.Invoke<TowerDestroyed>(new TowerDestroyed());
            }
        }

        public void Heal()
        {
            
        }
    }
    
    public struct HealthChange
    {
        public float Current;
        public float Max;

        public HealthChange(float current, float max)
        {
            Current = current;
            Max = max;
        }
    }
    
    public struct TowerDestroyed{}
}