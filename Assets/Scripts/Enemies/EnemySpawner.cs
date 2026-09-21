using System.Collections.Generic;
using Enemies.Factories;
using ScriptableObjectData.EnemySO;
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
            return _factory.Create(type, point.position);
        }
    }
}