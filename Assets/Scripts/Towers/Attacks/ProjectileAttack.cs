using Enemies;
using Game.Chances;
using Lean.Pool;
using Towers.Combat;
using Towers.Projectiles;
using UnityEngine;

namespace Towers.Attacks
{
    public class ProjectileAttack : ITowerAttack
    {
        private const float MinAttackSpeed = 0.01f;

        private readonly float _multishotDelay;
        private readonly IChanceRoller _chanceRoller = new ChanceRoller();

        private TowerRuntimeStats _stats;
        private TowerDamageDealer _damageDealer;
        private Transform _origin;

        private float _cooldown;
        private float _extraShotTimer;
        private bool _hasExtraShot;
        private Enemy _extraShotTarget;

        public ProjectileAttack(float multishotDelay)
        {
            _multishotDelay = multishotDelay;
        }

        public void Init(TowerRuntimeStats stats, TowerDamageDealer damageDealer, Transform origin)
        {
            _stats = stats;
            _damageDealer = damageDealer;
            _origin = origin;
        }

        public void Tick(Enemy target)
        {
            _cooldown -= Time.deltaTime;
            UpdateExtraShot(target);

            if (target == null || _cooldown > 0f) return;

            float attackInterval = 1f / Mathf.Max(MinAttackSpeed, _stats.Stats.AttackSpeed);

            Fire(target);
            TryScheduleExtraShot(target, attackInterval);

            _cooldown = attackInterval;
        }

        public void Stop()
        {
            _hasExtraShot = false;
            _extraShotTarget = null;
        }

        private void TryScheduleExtraShot(Enemy target, float attackInterval)
        {
            if (!_chanceRoller.Roll(_stats.Stats.MultishotChance)) return;

            _hasExtraShot = true;
            _extraShotTarget = target;
            _extraShotTimer = Mathf.Min(_multishotDelay, attackInterval * 0.5f);
        }

        private void UpdateExtraShot(Enemy currentTarget)
        {
            if (!_hasExtraShot) return;

            _extraShotTimer -= Time.deltaTime;
            if (_extraShotTimer > 0f) return;

            _hasExtraShot = false;

            Enemy finalTarget = IsAlive(_extraShotTarget) ? _extraShotTarget : currentTarget;
            _extraShotTarget = null;

            if (finalTarget != null)
            {
                Fire(finalTarget);
            }
        }

        private void Fire(Enemy target)
        {
            Projectile projectile = LeanPool.Spawn(_stats.TowerData.Projectile, _origin.position, Quaternion.identity);
            projectile.Init(target, _damageDealer.CreateDamage(), _damageDealer);
        }

        private static bool IsAlive(Enemy enemy)
        {
            return enemy != null && enemy.gameObject.activeInHierarchy && !enemy.Health.IsDead;
        }
    }
}