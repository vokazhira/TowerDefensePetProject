using UnityEngine;

namespace Enemies.Abilities
{
    public abstract class EnemyAbility : MonoBehaviour
    {
        protected Enemy Owner { get; private set; }
        protected EnemyContext Context { get; private set; }

        public virtual void Init(Enemy owner, EnemyContext context)
        {
            Owner = owner;
            Context = context;
        }
        
        public virtual void OnKilled() { }

        protected virtual void OnDisable()
        {
            Context = null;
        }
    }
}