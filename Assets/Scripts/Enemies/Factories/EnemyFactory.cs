using System.Collections.Generic;
using Lean.Pool;
using ScriptableObjectData.EnemySO;
using Towers;
using UnityEngine;

namespace Enemies.Factories
{
    public class EnemyFactory
    {
        private Dictionary<EnemyType, EnemyData> _dataByType = new Dictionary<EnemyType, EnemyData>();
        private Transform _towerTransform;

        public void Init(IEnumerable<EnemyData> enemyDataList, Transform towerTransform)
        {
            _towerTransform = towerTransform;
            _dataByType.Clear();
            
            foreach (EnemyData data in enemyDataList)
            {
                if (data != null && !_dataByType.ContainsKey(data.Type))
                {
                    _dataByType.Add(data.Type, data);
                }
            }
        }

        public Enemy Create(EnemyType type, Vector3 position)
        {
            if (!_dataByType.TryGetValue(type, out EnemyData data))
            {
                Debug.LogError($"EnemyFactory: нет данных для типа {type}");
                return null;
            }
            
            Enemy enemy = LeanPool.Spawn(data.Prefab, position, Quaternion.identity);
            enemy.Init(data.Stats, _towerTransform);
            return enemy;
        }
    }
}