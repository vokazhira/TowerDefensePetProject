namespace Game.Waves
{
    public readonly struct LevelResult
    {
        public int Crystals { get; }
        public int CompletedWaves { get; }
        public int TotalWaves { get; }

        public LevelResult(int crystals, int completedWaves, int totalWaves)
        {
            Crystals = crystals;
            CompletedWaves = completedWaves;
            TotalWaves = totalWaves;
        }
    }
}