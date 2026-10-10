using System.Collections;
using Lean.Pool;
using UnityEngine;

namespace Enemies.Abilities
{
    public class ArcherAbility : EnemyAbility
    {
        [SerializeField] private float[] _shotDistances = { 8f, 5f };
        [SerializeField, Min(0f)] private float _aimTime = 0.5f;
        [SerializeField, Min(0f)] private float _afterShotDelay = 0.3f;
        [SerializeField, Min(0f)] private float _shotDamage = 20f;
        [SerializeField] private EnemyProjectile _arrowPrefab;

        private int _nextShotIndex;
        private bool _isShooting;

        public override void Init(Enemy owner, EnemyContext context)
        {
            base.Init(owner, context);
            _nextShotIndex = 0;
            _isShooting = false;
        }

        private void Update()
        {
            if (Context == null || _isShooting) return;
            if (_nextShotIndex >= _shotDistances.Length) return;

            float distance = Vector2.Distance(transform.position, Context.Tower.position);

            if (distance <= _shotDistances[_nextShotIndex])
            {
                StartCoroutine(ShootRoutine());
            }
        }

        private IEnumerator ShootRoutine()
        {
            _isShooting = true;
            Owner.Movement.SetPaused(true);

            yield return new WaitForSeconds(_aimTime);
            Fire();
            _nextShotIndex++;

            yield return new WaitForSeconds(_afterShotDelay);

            Owner.Movement.SetPaused(false);
            _isShooting = false;
        }

        private void Fire()
        {
            if (Context == null || Context.TowerTarget == null) return;

            EnemyProjectile arrow = LeanPool.Spawn(_arrowPrefab, transform.position, Quaternion.identity);
            arrow.Init(Context.Tower, Context.TowerTarget, _shotDamage);
        }
    }
}