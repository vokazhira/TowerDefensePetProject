using ScriptableObjectData.EnemySO;
using UnityEngine;

namespace Enemies.Abilities
{
    public class SummonerAbility : EnemyAbility
    {
        [SerializeField] private EnemyData _summonData;
        [SerializeField, Min(0.1f)] private float _interval = 2f;
        [SerializeField, Min(0f)] private float _spawnRadius = 0.6f;

        private float _timer;

        public override void Init(Enemy owner, EnemyContext context)
        {
            base.Init(owner, context);
            _timer = _interval;
        }

        private void Update()
        {
            if (Context == null) return;

            _timer -= Time.deltaTime;
            if (_timer > 0f) return;

            _timer = _interval;
            Summon();
        }

        private void Summon()
        {
            Vector3 offset = Random.insideUnitCircle.normalized * _spawnRadius;
            Context.Factory.Create(_summonData, transform.position + offset);
        }
    }
}