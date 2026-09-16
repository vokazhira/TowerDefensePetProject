using System.Collections;
using System.Collections.Generic;
using Enemies;
using Lean.Pool;
using ScriptableObjects;
using Towers.Projectiles;
using UnityEngine;

namespace Towers
{
    public class TowerLogic : MonoBehaviour
    {
        [SerializeField] private TowerDataSO _towerData;

        private TowerHealth _towerHealth;
        private CircleCollider2D _rangeCollider;
        private List<Enemy> _enemiesInRange = new List<Enemy>();

        private Enemy _currentTarget;
        private int _shotsFiredAtTarget;
        private Coroutine _shootRoutine;

        private void Awake()
        {
            _towerHealth = GetComponent<TowerHealth>();
            _rangeCollider = GetComponent<CircleCollider2D>();
            
            TowerStats stats = _towerData.TowerStats;
            _towerHealth.Init(stats.MaxHealth, stats.Defense, stats.HealthRegeneration);
            
            _rangeCollider.isTrigger = true;
            _rangeCollider.radius = _towerData.TowerStats.Range;
        }
        
        private void Update()
        {
            if (!IsTargetValid(_currentTarget))
            {
                _currentTarget = SelectTarget();
                _shotsFiredAtTarget = 0;
            }

            if (_currentTarget != null && _shootRoutine == null)
            {
                _shootRoutine = StartCoroutine(ShootRoutine());
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (!_enemiesInRange.Contains(enemy))
                {
                    _enemiesInRange.Add(enemy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                _enemiesInRange.Remove(enemy);

                if (enemy == _currentTarget)
                {
                    _currentTarget = null;
                    _shotsFiredAtTarget = 0;
                }
            }
        }

        private Enemy SelectTarget()
        {
            CleanUpEnemies();
            
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
        
        private void Shoot(Enemy target)
        {
            if (target == null) return;
            
            Projectile proj = LeanPool.Spawn(_towerData.Projectile, transform.position, Quaternion.identity);
            proj.Init(target, _towerData.TowerStats.Damage);
        }
        
        private void CleanUpEnemies() => _enemiesInRange.RemoveAll(e => 
            e == null || !e.gameObject.activeInHierarchy || e.Health.IsDead);
        private bool IsTargetValid(Enemy enemy) => enemy != null 
                                                   && enemy.gameObject.activeInHierarchy
                                                   && !enemy.Health.IsDead
                                                   && _enemiesInRange.Contains(enemy);
        private int GetKillShotsCount(Enemy target) => Mathf.CeilToInt(target.Health.MaxHealth / _towerData.TowerStats.Damage);
        private float GetAttackInterval() => 1f / _towerData.TowerStats.AttackSpeed;

        private IEnumerator ShootRoutine()
        {
            while (true)
            {
                if (!IsTargetValid(_currentTarget))
                {
                    _currentTarget = SelectTarget();
                    if (_currentTarget == null)
                    {
                        _shootRoutine = null;
                        yield break;
                    }
                }
                
                Shoot(_currentTarget);
                _shotsFiredAtTarget++;

                if (_shotsFiredAtTarget >= GetKillShotsCount(_currentTarget))
                {
                    _currentTarget = SelectTarget();
                    _shotsFiredAtTarget = 0;

                    if (_currentTarget == null)
                    {
                        _shootRoutine = null;
                        yield break;
                    }
                }
                yield return new WaitForSeconds(GetAttackInterval());
            }
        }
    }
}