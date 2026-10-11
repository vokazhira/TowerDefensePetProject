using UnityEngine;

namespace ScriptableObjectData.LevelSO
{
    [CreateAssetMenu(fileName = "LevelCatalog", menuName = "ScriptableObjects/LevelCatalog", order = 55)]
    public class LevelCatalogSO : ScriptableObject
    {
        [SerializeField] private LevelData[] _levels;

        public int Count => _levels.Length;

        public LevelData Get(int index) => _levels[index];

        public int IndexOf(LevelData level)
        {
            for (int i = 0; i < _levels.Length; i++)
            {
                if (_levels[i] == level) return i;
            }

            return -1;
        }
    }
}