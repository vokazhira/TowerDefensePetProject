using System;
using UnityEngine;

namespace Enemies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 52)]
    public class EnemyData : ScriptableObject
    {
        public EnemyType Type;
        public Enemy Prefab;
        public EnemyStats Stats;
    }
}