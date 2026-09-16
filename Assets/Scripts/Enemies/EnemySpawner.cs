using System.Collections.Generic;
using Enemies.Factories;
using Enemies.ScriptableObjects;
using GameData.Observer;
using Lean.Pool;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private List<EnemyData> _enemyDatas;
        [SerializeField] private Transform _towerTransform;

        private EnemyFactory _factory = new EnemyFactory();

        private void Awake()
        {
            _factory.Init(_enemyDatas, _towerTransform);
        }

        public Enemy Spawn(EnemyType type)
        {
            Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            Enemy enemy = _factory.Create(type, point.position);
            
            GameEvents.NotifyEnemySpawned(enemy);

            return enemy;
        }
    }
}