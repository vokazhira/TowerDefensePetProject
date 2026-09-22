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
            Vector2 point = (Vector2)_spawnPoints[Random.Range(0, _spawnPoints.Length)].position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            return _factory.Create(type, point);
        }
    }
}