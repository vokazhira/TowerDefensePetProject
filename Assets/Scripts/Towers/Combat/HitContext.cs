using Enemies;

namespace Towers.Combat
{
    public readonly struct HitContext
    {
        public readonly Enemy Target;
        public readonly DamageInfo Damage;
        public readonly TowerStats Stats;

        public HitContext(Enemy target, DamageInfo damage, TowerStats stats)
        {
            Target = target;
            Damage = damage;
            Stats = stats;
        }
    }
}