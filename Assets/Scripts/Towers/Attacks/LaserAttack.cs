using Enemies;
using Towers.Combat;
using UnityEngine;

namespace Towers.Attacks
{
    public class LaserAttack : ITowerAttack
    {
        private const float MinAttackSpeed = 0.01f;

        private readonly LaserBeam _beamPrefab;
        private readonly float _maxDamageMultiplier;
        private readonly float _rampUpTime;

        private TowerRuntimeStats _stats;
        private TowerDamageDealer _damageDealer;
        private Transform _origin;
        private LaserBeam _beam;

        private Enemy _currentTarget;
        private float _focusTime;
        private float _tickTimer;

        public LaserAttack(LaserBeam beamPrefab, float maxDamageMultiplier, float rampUpTime)
        {
            _beamPrefab = beamPrefab;
            _maxDamageMultiplier = maxDamageMultiplier;
            _rampUpTime = rampUpTime;
        }

        public void Init(TowerRuntimeStats stats, TowerDamageDealer damageDealer, Transform origin)
        {
            _stats = stats;
            _damageDealer = damageDealer;
            _origin = origin;

            _beam = Object.Instantiate(_beamPrefab, origin);
            _beam.Hide();
        }

        public void Tick(Enemy target)
        {
            if (target == null)
            {
                Stop();
                return;
            }

            if (target != _currentTarget)
            {
                _currentTarget = target;
                _focusTime = 0f;
            }

            _focusTime += Time.deltaTime;
            float power = GetRampProgress();

            _beam.Show(_origin.position, target.transform.position, power);

            _tickTimer -= Time.deltaTime;
            if (_tickTimer > 0f) return;

            _tickTimer = 1f / Mathf.Max(MinAttackSpeed, _stats.Stats.AttackSpeed);
            DealDamage(target, power);
        }

        public void Stop()
        {
            _currentTarget = null;
            _focusTime = 0f;

            if (_beam != null)
            {
                _beam.Hide();
            }
        }

        private float GetRampProgress()
        {
            if (_rampUpTime <= 0f) return 1f;
            return Mathf.Clamp01(_focusTime / _rampUpTime);
        }

        private void DealDamage(Enemy target, float power)
        {
            DamageInfo baseDamage = _damageDealer.CreateDamage();
            float multiplier = Mathf.Lerp(1f, _maxDamageMultiplier, power);

            _damageDealer.HandleHit(target, new DamageInfo(baseDamage.Amount * multiplier, baseDamage.IsCritical));
        }
    }
}