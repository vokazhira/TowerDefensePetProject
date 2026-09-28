namespace Upgrades.Prices
{
    public class ResourceUpgradePriceStrategy : IUpgradePriceStrategy
    {
        private int _startPrice;
        private int _firstStep;
        private int _stepGrowth;

        public ResourceUpgradePriceStrategy(int startPrice, int firstStep, int stepGrowth)
        {
            _startPrice = startPrice;
            _firstStep = firstStep;
            _stepGrowth = stepGrowth;
        }
        
        public int GetPrice(int boughtUpgradeCount)
        {
            int price = _startPrice;
            int step = _firstStep;

            for (int i = 0; i < boughtUpgradeCount; i++)
            {
                price += step;
                step += _stepGrowth;
            }
            
            return price;
        }
    }
}