namespace Towers.Combat
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly bool IsCritical;

        public DamageInfo(float amount, bool isCritical = false)
        {
            Amount = amount;
            IsCritical = isCritical;
        }
    }
}