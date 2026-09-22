using Game.Currency;
using Game.Waves;
using ScriptableObjectData.TowerSO;
using Towers;
using UI;
using UnityEngine;

namespace Game
{
    public class BattleBootstrap : MonoBehaviour
    {
        [SerializeField] private TowerDataSO _normalTowerData;
        [SerializeField] private TowerRuntimeStats _towerRuntimeStats;
        [SerializeField] private TowerLogic _towerLogic;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private UpgradePanelView _battleUpgradePanel;
        
        private GameCurrency _gameCurrency;

        private void Start()
        {
            _gameCurrency = new GameCurrency();
            
            _towerRuntimeStats.Init(_normalTowerData);
            _towerLogic.Init(_towerRuntimeStats);
            
            _levelView.Init(_gameCurrency);
            _battleUpgradePanel.Init(_gameCurrency);

            _levelController.Init(_gameCurrency);
            _levelController.StartSelectedLevel();
        }

        private void OnDestroy()
        {
            _gameCurrency.Dispose();
        }
    }
}