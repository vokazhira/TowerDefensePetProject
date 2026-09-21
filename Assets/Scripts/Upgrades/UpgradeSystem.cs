using Game.States;
using ScriptableObjectData.TowerSO;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeSystem : MonoBehaviour
    {
        [SerializeField] private Button _buttonPrefab;
        
        private TowerDataSO _towerData;
        private GameState _gameState;

        public void UpgradeStat()
        {
            
        }
    }
}