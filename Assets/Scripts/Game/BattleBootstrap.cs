using Game.Chances;
using Game.Currency;
using Game.Waves;
using ScriptableObjectData.TowerSO;
using Towers;
using Towers.Combat;
using UI;
using UnityEngine;

namespace Game
{
    public class BattleBootstrap : MonoBehaviour
    {
        [SerializeField] private TowerDataSO _normalTowerData;
        [SerializeField] private TowerRuntimeStats _towerRuntimeStats;
        [SerializeField] private TowerLogic _towerLogic;
        [SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private UpgradePanelView _battleUpgradePanel;

        private GameCurrency _gameCurrency;
        
        private void Start()
        {
            _gameCurrency = new GameCurrency();

            _towerRuntimeStats.Init(_normalTowerData);
            TowerDamageDealerFactory dealerFactory = new TowerDamageDealerFactory(new ChanceRoller());
            TowerDamageDealer damageDealer = dealerFactory.Create(_towerRuntimeStats, _towerHealth);
            
            _towerLogic.Init(_towerRuntimeStats, damageDealer);

            _levelView.Init(_gameCurrency);
            _battleUpgradePanel.Init(_gameCurrency);
            
            _levelController.StartSelectedLevel(_towerRuntimeStats);
        }

        private void OnDestroy()
        {
            _gameCurrency.Dispose();
        }
    }
}