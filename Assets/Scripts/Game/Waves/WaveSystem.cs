using System.Collections;
using Enemies;
using Game.Currency;
using Game.Events;
using Interfaces;
using ScriptableObjectData.LevelSO;
using ScriptableObjectData.WaveSO;
using UnityEngine;

namespace Game.Waves
{
    public class WaveSystem : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _spawner;
        
        private LevelData _currentLevel;
        private IResourceRewardSource _rewards;
        private int _currentWaveIndex;
        private int _aliveEnemies;
        private bool _isSpawning;
        private bool _isRunning;
        private int _crystalEarnedThisLevel;
        
        public int CrystalEarnedThisLevel => _crystalEarnedThisLevel;

        private void OnEnable()
        {
            EventBus.Subscribe<EnemyDied>(OnEnemydied);
            EventBus.Subscribe<EnemySpawned>(OnEnemySpawned);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<EnemyDied>(OnEnemydied);
            EventBus.Unsubscribe<EnemySpawned>(OnEnemySpawned);
        }

        public void StartLevel(LevelData level, IResourceRewardSource rewards)
        {
            if (_isRunning || level == null || level.Waves.Count == 0) return;
            
            _currentLevel = level;
            _rewards = rewards;
            _isRunning = true;
            _currentWaveIndex = 0;
            _aliveEnemies = 0;
            _crystalEarnedThisLevel = 0;

            StartCoroutine(RunLevel());
        }

        public void StopLevel()
        {
            StopAllCoroutines();
            _isRunning = false;
            _isSpawning = false;
            _currentLevel = null;
        }

        private IEnumerator RunLevel()
        {
            while (_currentWaveIndex < _currentLevel.Waves.Count)
            {
                WaveData wave = _currentLevel.Waves[_currentWaveIndex];
                yield return new WaitForSeconds(wave.DelayBeforeWave);

                EventBus.Invoke<WaveChange>(new WaveChange(_currentWaveIndex + 1, _currentLevel.Waves.Count));
                
                yield return StartCoroutine(SpawnWave(wave));
                
                yield return new WaitUntil(() => _aliveEnemies <= 0 && !_isSpawning);

                GetWaveRewards();
                
                _currentWaveIndex++;
            }

            int fullClearBonus = _crystalEarnedThisLevel;
            _crystalEarnedThisLevel += fullClearBonus;
            EventBus.Invoke(new CrystalsEarned(fullClearBonus));

            EventBus.Invoke(new AllWavesCompleted());
            _isRunning = false;
            _currentLevel = null;
        }

        private void GetWaveRewards()
        {
            int crystalBonus = _rewards != null ? _rewards.WaveCrystalReward : 0;
            int crystalForWave = _currentLevel.CrystalPerWave + crystalBonus;
            
            _crystalEarnedThisLevel += crystalForWave;
            EventBus.Invoke<CrystalsEarned>(new CrystalsEarned(crystalForWave));
            
            int goldForWave = _rewards != null ? _rewards.WaveGoldReward : 0;

            if (goldForWave > 0)
            {  
                EventBus.Invoke<GoldEarned>(new GoldEarned(goldForWave));
            }
        }

        private IEnumerator SpawnWave(WaveData wave)
        {
            _isSpawning = true;

            foreach (WaveContent content in wave.Contents)
            {
                for (int i = 0; i < content.Count; i++)
                {
                   _spawner.Spawn(content.EnemyType);
                   yield return new WaitForSeconds(content.DelayBetweenSpawns);
                }
            }
            
            _isSpawning = false;
        }
        
        private void OnEnemySpawned(EnemySpawned spawned)
        {
            _aliveEnemies++;
        }

        private void OnEnemydied(EnemyDied died)
        {
            _aliveEnemies = Mathf.Max(0, _aliveEnemies - 1);

            int killBonus = _rewards != null ? _rewards.GoldPerKillBonus : 0;
            
            if (killBonus > 0) EventBus.Invoke<GoldEarned>(new GoldEarned(killBonus));
        }
    }

    public struct WaveChange
    {
        public int Current;
        public int Total;

        public WaveChange(int current, int total)
        {
            Current = current;
            Total = total;
        }
    }
    
    public struct AllWavesCompleted{}
}