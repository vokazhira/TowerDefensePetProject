namespace Upgrades.Prices
{
    public class DefaultPriceStrategy : IUpgradePriceStrategy
    {
        public int GetPrice(int boughtUpgradeCount)
        {
            return UpgradePriceCalculator.GetPrice(boughtUpgradeCount);
        }
    }
}