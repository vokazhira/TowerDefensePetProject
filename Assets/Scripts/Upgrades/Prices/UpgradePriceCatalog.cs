using System.Collections.Generic;
using Towers;

namespace Upgrades.Prices
{
    public class UpgradePriceCatalog
    {
        private IUpgradePriceStrategy _defaultStrategy = new DefaultPriceStrategy();
        private Dictionary<TowerStatType, IUpgradePriceStrategy> _strategies;

        public UpgradePriceCatalog()
        {
            IUpgradePriceStrategy resourcePrice = new ResourceUpgradePriceStrategy(21, 31, 3);

            _strategies = new Dictionary<TowerStatType, IUpgradePriceStrategy>
            {
                { TowerStatType.WaveGoldReward, new WaveGoldRewardPriceStrategy() },
                { TowerStatType.WaveCrystalReward, resourcePrice },
                { TowerStatType.GoldPerKillBonus, resourcePrice },
                { TowerStatType.FreeUpgradeChance, resourcePrice },
            };
        }

        public int GetPrice(TowerStatType statType, int boughtUpgradeCount)
        {
            IUpgradePriceStrategy strategy;
            IUpgradePriceStrategy found;

            if (_strategies.TryGetValue(statType, out found))
            {
                strategy = found;
            }
            else
            {
                strategy = _defaultStrategy;
            }
            
            return strategy.GetPrice(boughtUpgradeCount);
        }
    }
}