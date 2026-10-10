using System.Collections.Generic;
using Game.Events;
using Interfaces.Damage;
using Lean.Pool;
using ScriptableObjectData.EnemySO;
using UnityEngine;

namespace Enemies.Factories
{
    public class EnemyFactory
    {
        private Dictionary<EnemyType, EnemyData> _dataByType = new Dictionary<EnemyType, EnemyData>();
        private EnemyContext _context;

        public void Init(IEnumerable<EnemyData> enemyDataList, Transform towerTransform)
        {
            IDamageable towerTarget = towerTransform.GetComponentInParent<IDamageable>();
            
            _context = new EnemyContext(towerTransform, towerTarget, this);
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
            
            return Create(data, position);
        }

        public Enemy Create(EnemyData data, Vector3 position)
        {
            if (data == null || data.Prefab == null)
            {
                Debug.LogError("EnemyFactory: EnemyData или его Prefab не назначен");
                return null;
            }

            Enemy enemy = LeanPool.Spawn(data.Prefab, position, Quaternion.identity);
            enemy.Init(data.Stats, _context);

            EventBus.Invoke(new EnemySpawned(enemy));
            return enemy;
        }
    }
}