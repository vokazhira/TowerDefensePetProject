using System;
using Enemies;
using ScriptableObjectData.LevelSO;

namespace Game.Events.Observer
{
    public class GameEvents
    {
        #region Enemies
        
        public static event Action<Enemy> OnEnemyDied;
        public static void NotifyEnemyDied(Enemy enemy) => OnEnemyDied?.Invoke(enemy);
        
        #endregion
        
        #region Waves
        
        public static event Action<LevelData> OnLevelStarted;
        public static event Action<int, int> OnWaveProgressChanged;
        public static event Action OnAllWavesCompleted;
        
        public static void NotifyLevelStarted(LevelData level) => OnLevelStarted?.Invoke(level);
        public static void NotifyWaveProgressChanged(int currentWave, int totalWave) => OnWaveProgressChanged?.Invoke(currentWave, totalWave);
        public static void NotifyAllWavesCompleted() => OnAllWavesCompleted?.Invoke();
        
        #endregion

        #region Tower
        
        public static event Action OnTowerDestroyed;
        public static void NotifyTowerDestroyed() => OnTowerDestroyed?.Invoke();

        #endregion
        
        #region Currency
        
        public static event Action<int> OnGoldEarned;
        public static event Action<int> OnCrystalsEarned;
        public static void NotifyGoldEarned(int amount) => OnGoldEarned?.Invoke(amount);
        public static void NotifyCrystalsEarned(int amount) => OnCrystalsEarned?.Invoke(amount);
        
        #endregion
    }
}