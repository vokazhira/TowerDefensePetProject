using System.Collections;
using System.Collections.Generic;
using Enemies;
using Game.Chances;
using Lean.Pool;
using Towers.Combat;
using Towers.Projectiles;
using UnityEngine;

namespace Towers
{
    public class TowerLogic : MonoBehaviour
    {
        private const float MinAttackSpeed = 0.01f;
        
        [SerializeField] private CircleCollider2D _rangeCollider;
        [SerializeField, Min(0f)] private float _multishotDelay = 0.12f;

        private TowerRuntimeStats _runtimeStats;
        private TowerHealth _towerHealth;
        private List<Enemy> _enemiesInRange = new List<Enemy>();

        private IChanceRoller _chanceRoller;
        private TowerDamageDealer _damageDealer;
        
        private Coroutine _shootRoutine;
        private Enemy _currentTarget;

        public void Init(TowerRuntimeStats runtimeStats, TowerDamageDealer damageDealer)
        {
            _runtimeStats = runtimeStats;
            _damageDealer = damageDealer;
            _towerHealth = GetComponent<TowerHealth>();
            _chanceRoller = new ChanceRoller();
            
            
            _towerHealth.ConfigureAtLevelStart(_runtimeStats.Stats);
            _rangeCollider.isTrigger = true;
            _rangeCollider.radius = _runtimeStats.Stats.Range;

            _runtimeStats.OnStatChange += HandleStatChange;
        }

        private void Update()
        {
            if (_runtimeStats == null) return;
            
            if (_currentTarget == null && _enemiesInRange.Count > 0)
            {
                _currentTarget = GetNearestEnemy();
            }

            if (_currentTarget != null && _shootRoutine == null)
            {
                _shootRoutine = StartCoroutine(ShootRoutine());
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
            while (IsAlive(_currentTarget))
            {
                float attackInterval = 1f / Mathf.Max(MinAttackSpeed, _runtimeStats.Stats.AttackSpeed);
                
                Fire(_currentTarget);
                TryMultishot(_currentTarget, attackInterval);

                yield return new WaitForSeconds(attackInterval);
                _currentTarget = GetNearestEnemy();
            }

            _shootRoutine = null;
        }

        private void TryMultishot(Enemy target, float attackInterval)
        {
            if (!_chanceRoller.Roll(_runtimeStats.Stats.MultishotChance)) return;

            float delay = Mathf.Min(_multishotDelay, attackInterval * 0.5f);
            StartCoroutine(FireExtraShot(target, delay));
        }

        private IEnumerator FireExtraShot(Enemy target, float delay)
        {
            yield return new WaitForSeconds(delay);

            Enemy finalTarget = IsAlive(target) ? target : GetNearestEnemy();

            if (finalTarget != null)
            {
                Fire(finalTarget);
            }
        }

        private void Fire(Enemy target)
        {
            Projectile projectile = LeanPool.Spawn(_runtimeStats.TowerData.Projectile, transform.position, Quaternion.identity);
            projectile.Init(target, _damageDealer.CreateDamage(), _damageDealer);
        }

        private Enemy GetNearestEnemy()
        {
            _enemiesInRange.RemoveAll(enemy => !IsAlive(enemy));
            
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

        private void OnDestroy()
        {
            if (_runtimeStats != null)
            {
                _runtimeStats.OnStatChange -= HandleStatChange;
            }
        }
    }
}