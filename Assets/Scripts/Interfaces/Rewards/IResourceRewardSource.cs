namespace Interfaces
{
    public interface IResourceRewardSource
    {
        public int WaveGoldReward { get; }
        public int WaveCrystalReward { get; }
        public int GoldPerKillBonus { get; }
    }
}