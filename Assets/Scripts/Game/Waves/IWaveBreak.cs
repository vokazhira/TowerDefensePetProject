namespace Game.Waves
{
    public interface IWaveBreak
    {
        public bool IsOnBreak { get; }
        public float BreakTimeLeft { get; }
        public void SkipBreak();
    }
}