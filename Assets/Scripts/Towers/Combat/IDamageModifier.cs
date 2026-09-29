namespace Towers.Combat
{
    public interface IDamageModifier
    {
        public DamageInfo Modify(DamageInfo damageInfo, TowerStats stats);
    }
}