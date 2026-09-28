namespace Upgrades.Prices
{
    public interface IUpgradePriceStrategy
    {
        int GetPrice(int boughtUpgradeCount);
    }
}