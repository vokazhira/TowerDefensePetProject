using Towers;
using Towers.Projectiles;
using UnityEngine;

namespace ScriptableObjectData.TowerSO
{
    [CreateAssetMenu(fileName = "TowerDataSO", menuName = "ScriptableObjects/TowerData", order = 51)]
    public class TowerDataSO : ScriptableObject
    {
        public string TowerName;
        public TowersType TowerType;
        public Sprite Sprite;
        public TowerStats TowerStats;
        public Projectile Projectile;
    }
}