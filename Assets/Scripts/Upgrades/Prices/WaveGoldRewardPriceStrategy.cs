namespace Upgrades.Prices
{
    public class WaveGoldRewardPriceStrategy : IUpgradePriceStrategy
    {
        public int GetPrice(int boughtUpgradeCount)
        {
            int nextLevel = boughtUpgradeCount + 1;
            return (nextLevel + 1) / 2;
        }
    }
}