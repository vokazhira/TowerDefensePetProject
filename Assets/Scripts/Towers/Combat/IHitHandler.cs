using Enemies;

namespace Towers.Combat
{
    public interface IHitHandler
    {
        void HandleHit(Enemy target, DamageInfo damage);
    }
}