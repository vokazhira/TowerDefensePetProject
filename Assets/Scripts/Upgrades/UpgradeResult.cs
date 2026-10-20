namespace Upgrades
{
    public readonly struct UpgradeResult
    {
        public bool IsSuccess { get; }
        public bool IsFree { get; }
        public float Amount { get; }

        public static UpgradeResult Failed => default;

        public UpgradeResult(bool isFree, float amount)
        {
            IsSuccess = true;
            IsFree = isFree;
            Amount = amount;
        }
    }
}