using Towers.Attacks;
using UnityEngine;

namespace ScriptableObjectData.AttackSO
{
    [CreateAssetMenu(fileName = "ProjectileAttack", menuName = "ScriptableObjects/Attacks/Projectile", order = 60)]
    public class ProjectileAttackSO : TowerAttackSO
    {
        [SerializeField, Min(0f)] private float _multishotDelay = 0.12f;

        public override ITowerAttack CreateAttack()
        {
            return new ProjectileAttack(_multishotDelay);
        } 
    }
}