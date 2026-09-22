using System.Collections;
using System.Collections.Generic;
using Enemies;
using Lean.Pool;
using Towers.Projectiles;
using UnityEngine;

namespace Towers
{
    public class TowerLogic : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _rangeCollider;

        private TowerRuntimeStats _runtimeStats;
        private TowerHealth _towerHealth;
        private List<Enemy> _enemiesInRange = new List<Enemy>();

        private Coroutine _shootRoutine;
        private Enemy _currentTarget;

        public void Init(TowerRuntimeStats runtimeStats)
        {
            _runtimeStats = runtimeStats;
            _towerHealth = GetComponent<TowerHealth>();
            
            _towerHealth.ConfigureAtLevelStart(_runtimeStats.Stats);
            _rangeCollider.isTrigger = true;
            _rangeCollider.radius = _runtimeStats.Stats.Range;

            _runtimeStats.OnStatChange += HandleStatChange;
        }

        private void Update()
        {
            if (_currentTarget == null && _enemiesInRange.Count > 0)
            {
                _currentTarget = GetNearestEnemy();
            }

            if (_currentTarget != null && _shootRoutine == null)
            {
                _shootRoutine = StartCoroutine("ShootRoutine");
            }
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

        private IEnumerator ShootRoutine()
        {
            while (_currentTarget != null && !_currentTarget.Health.IsDead)
            {
                Projectile projectile = LeanPool.Spawn(_runtimeStats.TowerData.Projectile,  transform.position, Quaternion.identity);
                projectile.Init(_currentTarget, _runtimeStats.Stats.Damage);

                yield return new WaitForSeconds(1f / _runtimeStats.Stats.AttackSpeed);
                _currentTarget = GetNearestEnemy();
            }

            _shootRoutine = null;
        }

        private Enemy GetNearestEnemy()
        {
            _enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeInHierarchy || enemy.Health.IsDead);
            
            Enemy nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (Enemy enemy in _enemiesInRange)
            {
                float distance  = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = enemy;
                }
            }
            
            return nearest;
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

        private void OnDestroy()
        {
            _runtimeStats.OnStatChange -= HandleStatChange;
        }
    }
}