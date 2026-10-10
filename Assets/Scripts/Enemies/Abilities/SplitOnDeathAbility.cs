using ScriptableObjectData.EnemySO;
using UnityEngine;

namespace Enemies.Abilities
{
    public class SplitOnDeathAbility : EnemyAbility
    {
        [SerializeField] private EnemyData _childData;
        [SerializeField, Min(1)] private int _minCount = 2;
        [SerializeField, Min(1)] private int _maxCount = 3;
        [SerializeField, Min(0f)] private float _spreadRadius = 0.5f;

        public override void OnKilled()
        {
            if (Context == null) return;

            int count = Random.Range(_minCount, _maxCount + 1);

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = Random.insideUnitCircle * _spreadRadius;
                Context.Factory.Create(_childData, transform.position + offset);
            }
        }
    }
}