using Enemies;
using Towers.Combat;
using UnityEngine;

namespace Towers.Attacks
{
    public interface ITowerAttack
    {
        public void Init(TowerRuntimeStats runtimeStats, TowerDamageDealer damageDealer, Transform origin);
        public void Tick(Enemy target);
        public void Stop();
    }
}