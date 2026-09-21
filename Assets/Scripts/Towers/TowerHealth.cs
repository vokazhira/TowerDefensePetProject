using System;
using Game.Events.Observer;
using Interfaces.Damage;
using ScriptableObjectData.LevelSO;
using UnityEngine;

namespace Towers
{
    public class TowerHealth : MonoBehaviour, IDamageable
    {
        private float _maxHealth;
        private float _defense;
        private float _regeneration;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => _maxHealth;
        public float Defense => _defense;
        public bool IsDead => CurrentHealth <= 0f;
        
        public event Action<float, float> OnHealthChanged;

        public void Init(float maxHealth, float defense, float regeneration)
        {
            _maxHealth = maxHealth;
            _defense = defense;
            _regeneration = regeneration;
            RestoreFullHealth();
        }

        private void OnEnable()
        {
            GameEvents.OnLevelStarted += HandleLevelStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnLevelStarted -= HandleLevelStarted;
        }

        private void Update()
        {
            if (IsDead || _regeneration <= 0f) return;

            if (CurrentHealth < _maxHealth)
            {
                SetHealth(CurrentHealth + _regeneration * Time.deltaTime);
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            float finalDamage = Mathf.Max(0f, damage - _defense);
            if (finalDamage <= 0f) return;

            SetHealth(CurrentHealth - finalDamage);

            if (IsDead) GameEvents.NotifyTowerDestroyed();
        }
        
        public void RestoreFullHealth()
        {
            SetHealth(_maxHealth);
        }
        
        private void HandleLevelStarted(LevelData levelData) => RestoreFullHealth();

        private void SetHealth(float value)
        {
            CurrentHealth = Mathf.Clamp(value, 0f, _maxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
        }
    }
}