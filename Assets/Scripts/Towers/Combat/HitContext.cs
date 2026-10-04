using Enemies;

namespace Towers.Combat
{
    public readonly struct HitContext
    {
        public Enemy Target { get; }
        public DamageInfo Damage { get; }
        public TowerStats Stats { get; }

        public HitContext(Enemy target, DamageInfo damage, TowerStats stats)
        {
            Target = target;
            Damage = damage;
            Stats = stats;
        }
    }
}