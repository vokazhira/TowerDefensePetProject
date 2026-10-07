using UnityEngine;

namespace ScriptableObjectData.TowerSO
{
    [CreateAssetMenu(fileName = "TowerCatalog", menuName = "ScriptableObjects/TowerCatalog", order = 52)]
    public class TowerCatalogSO : ScriptableObject
    {
        [SerializeField] private TowerDataSO[] _towers;
        
        public int Count => _towers.Length;
        
        public TowerDataSO Get(int index) => _towers[index];
        
        public int IndexOf(TowerDataSO tower)
        {
            for (int i = 0; i < _towers.Length; i++)
            {
                if (_towers[i] == tower) return i;
            }

            return -1;
        }
    }
}