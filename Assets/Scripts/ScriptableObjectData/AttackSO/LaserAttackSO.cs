using Towers.Attacks;
using UnityEngine;

namespace ScriptableObjectData.AttackSO
{
    [CreateAssetMenu(fileName = "LaserAttack", menuName = "ScriptableObjects/Attacks/Laser", order = 61)]
    public class LaserAttackSO : TowerAttackSO
    {
        [SerializeField] private LaserBeam _beamPrefab;
        [SerializeField, Min(1f)] private float _maxDamageMultiplier = 3f;
        [SerializeField, Min(0f)] private float _rampUpTime = 3f;

        public override ITowerAttack CreateAttack()
        {
            return new LaserAttack(_beamPrefab, _maxDamageMultiplier, _rampUpTime);
        }
    }
}