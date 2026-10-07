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
        [SerializeField] private TowerDataSO _defaultTowerData;
        [SerializeField] private SpriteRenderer _towerSpriteRenderer;
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
            
            TowerDataSO towerData = GetSelectedTower();
            ApplyTowerSprite(towerData);

            _towerRuntimeStats.Init(towerData);
            TowerDamageDealerFactory dealerFactory = new TowerDamageDealerFactory(new ChanceRoller());
            TowerDamageDealer damageDealer = dealerFactory.Create(_towerRuntimeStats, _towerHealth);
            
            _towerLogic.Init(_towerRuntimeStats, damageDealer);

            _levelView.Init(_gameCurrency);
            _battleUpgradePanel.Init(_gameCurrency);
            
            _levelController.StartSelectedLevel(_towerRuntimeStats);
        }
        
        private TowerDataSO GetSelectedTower()
        {
            TowerDataSO selected = GameSession.Instance.SelectedTower;
            return selected != null ? selected : _defaultTowerData;
        }
        
        private void ApplyTowerSprite(TowerDataSO towerData)
        {
            if (towerData.Sprite != null)
            {
                _towerSpriteRenderer.sprite = towerData.Sprite;
            }
        }

        private void OnDestroy()
        {
            _gameCurrency.Dispose();
        }
    }
}