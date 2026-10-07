using Towers;
using Towers.Projectiles;
using UnityEngine;

namespace ScriptableObjectData.TowerSO
{
    [CreateAssetMenu(fileName = "TowerDataSO", menuName = "ScriptableObjects/TowerData", order = 51)]
    public class TowerDataSO : ScriptableObject
    {
        public string TowerName;
        [TextArea(3, 6)] public string Description;
        public TowersType TowerType;
        public Sprite Sprite;
        public TowerStats TowerStats;
        public TowerStatsMultiplier Multipliers;
        public Projectile Projectile;
    }
}