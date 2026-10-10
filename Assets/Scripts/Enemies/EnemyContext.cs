using Enemies.Factories;
using Interfaces.Damage;
using UnityEngine;

namespace Enemies
{
    public class EnemyContext
    {
        public Transform Tower { get; }
        public IDamageable TowerTarget  { get; }
        public EnemyFactory Factory { get; }

        public EnemyContext(Transform tower, IDamageable towerTarget , EnemyFactory factory)
        {
            Tower = tower;
            TowerTarget = towerTarget;
            Factory = factory;
        }
    }
}