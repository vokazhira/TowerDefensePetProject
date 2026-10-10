using System.Collections.Generic;
using Enemies;
using Towers.Attacks;
using UnityEngine;

namespace Towers
{
    public class TowerLogic : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _rangeCollider;

        private TowerRuntimeStats _runtimeStats;
        private TowerHealth _towerHealth;
        private ITowerAttack _attack;

        private readonly List<Enemy> _enemiesInRange = new List<Enemy>();
        private Enemy _currentTarget;

        public void Init(TowerRuntimeStats runtimeStats, ITowerAttack attack)
        {
            _runtimeStats = runtimeStats;
            _attack = attack;
            _towerHealth = GetComponent<TowerHealth>();

            _towerHealth.ConfigureAtLevelStart(_runtimeStats.Stats);
            _rangeCollider.isTrigger = true;
            _rangeCollider.radius = _runtimeStats.Stats.Range;

            _runtimeStats.OnStatChange += HandleStatChange;
        }

        private void Update()
        {
            if (_attack == null) return;

            if (!IsValidTarget(_currentTarget))
            {
                _currentTarget = GetNearestEnemy();
            }

            _attack.Tick(_currentTarget);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy) && !_enemiesInRange.Contains(enemy))
            {
                _enemiesInRange.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }

        private bool IsValidTarget(Enemy enemy)
        {
            return IsAlive(enemy) && _enemiesInRange.Contains(enemy);
        }

        private Enemy GetNearestEnemy()
        {
            _enemiesInRange.RemoveAll(enemy => !IsAlive(enemy));

            Enemy nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (Enemy enemy in _enemiesInRange)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = enemy;
                }
            }

            return nearest;
        }

        private static bool IsAlive(Enemy enemy)
        {
            return enemy != null && enemy.gameObject.activeInHierarchy && !enemy.Health.IsDead;
        }

        private void HandleStatChange(TowerStatType statType)
        {
            if (statType == TowerStatType.Range)
            {
                _rangeCollider.radius = _runtimeStats.Stats.Range;
            }

            if (statType == TowerStatType.MaxHealth ||
                statType == TowerStatType.Defense ||
                statType == TowerStatType.HealthRegeneration)
            {
                _towerHealth.ApplyStats(_runtimeStats.Stats);
            }
        }

        private void OnDisable()
        {
            _attack?.Stop();
        }

        private void OnDestroy()
        {
            if (_runtimeStats != null)
            {
                _runtimeStats.OnStatChange -= HandleStatChange;
            }
        }
    }
}