namespace Towers.Combat
{
    public interface IHitEffect
    {
        public void Apply(in HitContext context);
    }
}