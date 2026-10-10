using Towers.Attacks;
using UnityEngine;

namespace ScriptableObjectData.AttackSO
{
    public abstract class TowerAttackSO : ScriptableObject
    {
        public abstract ITowerAttack CreateAttack();
    }
}