using GameData.Observer;
using Interfaces.Damage;
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

        public void Init(float maxHealth, float defense, float regeneration)
        {
            _maxHealth = maxHealth;
            _defense = defense;
            _regeneration = regeneration;
            CurrentHealth = maxHealth;
        }

        private void Update()
        {
            if (IsDead || _regeneration <= 0f) return;

            if (CurrentHealth < _maxHealth)
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth + _regeneration * Time.deltaTime, 0f, _maxHealth);
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            float finalDamage = Mathf.Max(0f, damage - _defense);
            if (finalDamage <= 0f) return;

            CurrentHealth = Mathf.Clamp(CurrentHealth - finalDamage, 0f, _maxHealth);
            GameEvents.NotifyTowerDamaged(finalDamage);

            if (IsDead) GameEvents.NotifyTowerDestroyed();
        }
    }
}