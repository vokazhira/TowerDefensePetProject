namespace Towers.Combat
{
    public readonly struct DamageInfo
    {
        public float Amount { get; }
        public bool IsCritical { get; }

        public DamageInfo(float amount, bool isCritical = false)
        {
            Amount = amount;
            IsCritical = isCritical;
        }
    }
}