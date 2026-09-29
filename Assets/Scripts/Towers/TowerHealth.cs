using System;
using Game.Events;
using Interfaces.Damage;
using UnityEngine;

namespace Towers
{
    public class TowerHealth : MonoBehaviour, IDamageable, IHealable
    {
        public float CurrentHealth { get; private set; }
        public float MaxHealth {get; private set;}
        
        private float _defensePercent;
        private float _regeneration;
        
        public bool IsDead => CurrentHealth <= 0f;

        public void ConfigureAtLevelStart(TowerStats towerStats)
        {
            ApplyStats(towerStats);
            CurrentHealth = MaxHealth;
            NotifyHealthChanged();
        }

        public void ApplyStats(TowerStats towerStats)
        {
            MaxHealth = towerStats.MaxHealth;
            _defensePercent = towerStats.Defense;
            _regeneration = towerStats.HealthRegeneration;
            
            CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
            NotifyHealthChanged();
        }

        private void Update()
        {
            if (_regeneration > 0f)
            {
                Heal(_regeneration * Time.deltaTime);
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead || damage <= 0f) return;
            
            float finalDamage = ReduceByDefense(damage);
            if (finalDamage <= 0f) return;

            CurrentHealth = Mathf.Max(0f, CurrentHealth - finalDamage);
            NotifyHealthChanged();

            if (CurrentHealth <= 0f)
            {
                EventBus.Invoke<TowerDestroyed>(new TowerDestroyed());
            }
        }
        
        public void Heal(float amount)
        {
            if (IsDead || amount <= 0f || CurrentHealth >= MaxHealth) return;
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
            NotifyHealthChanged();
        }
        
        private float ReduceByDefense(float damage)
        {
            float defensePercent = TowerStatRules.Clamp(TowerStatType.Defense, _defensePercent);
            return damage * (1f - defensePercent / 100f);
        }
        
        private void NotifyHealthChanged()
        {
            EventBus.Invoke(new HealthChange(CurrentHealth, MaxHealth));
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