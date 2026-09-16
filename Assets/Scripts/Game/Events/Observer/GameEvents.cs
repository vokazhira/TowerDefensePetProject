using System;
using Enemies;

namespace GameData.Observer
{
    public class GameEvents
    {
        public static event Action<Enemy> OnEnemySpawned;
        public static event Action<Enemy> OnEnemyDied;
        public static event Action<int> OnGoldEarned;
        
        public static event Action OnWaveStarted;
        public static event Action OnWaveCompleted;
        public static event Action OnAllWavesCompleted;

        public static event Action<float> OnTowerDamaged;
        public static event Action OnTowerDestroyed;

        public static void NotifyEnemySpawned(Enemy enemy) => OnEnemySpawned?.Invoke(enemy);
        public static void NotifyEnemyDied(Enemy enemy) => OnEnemyDied?.Invoke(enemy);
        public static void NotifyGoldEarned(int amount) => OnGoldEarned?.Invoke(amount);

        public static void NotifyWaveStarted() => OnWaveStarted?.Invoke();
        public static void NotifyWaveCompleted() => OnWaveCompleted?.Invoke();
        public static void NotifyAllWavesCompleted() => OnAllWavesCompleted?.Invoke();

        public static void NotifyTowerDamaged(float damage) => OnTowerDamaged?.Invoke(damage);
        public static void NotifyTowerDestroyed() => OnTowerDestroyed?.Invoke();
    }
}