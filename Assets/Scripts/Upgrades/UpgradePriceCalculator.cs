namespace Upgrades
{
    public static class UpgradePriceCalculator
    {
        public static int GetPrice(int boughtUpgradeCount)
        {
            int price = 6;
            for (int i = 1; i <= boughtUpgradeCount; i++)
            {
                price += 16 + 2 * ((i - 1) / 2);
            }
            
            return price;
        }
    }
}