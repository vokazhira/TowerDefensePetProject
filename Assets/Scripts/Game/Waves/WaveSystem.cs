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
    public class WaveSystem : MonoBehaviour, IWaveBreak
    {
        [SerializeField] private EnemySpawner _spawner;
        
        private LevelData _currentLevel;
        private IResourceRewardSource _rewards;
        private int _currentWaveIndex;
        private int _aliveEnemies;
        private bool _isSpawning;
        private bool _isRunning;
        private int _crystalEarnedThisLevel;
        private int _completedWaves;
        private int _totalWaves;
        private bool _isOnBreak;
        private float _breakTimeLeft;
        
        public int CrystalEarnedThisLevel => _crystalEarnedThisLevel;
        public int CompletedWaves => _completedWaves;
        public int TotalWaves => _totalWaves;
        public bool IsOnBreak => _isOnBreak;
        public float BreakTimeLeft => _breakTimeLeft;

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
            _completedWaves = 0;
            _totalWaves = level.Waves.Count;

            StartCoroutine(RunLevel());
        }

        public void StopLevel()
        {
            StopAllCoroutines();
            EndBreak();
            _isRunning = false;
            _isSpawning = false;
            _currentLevel = null;
        }
        
        public void SkipBreak()
        {
            if (_isOnBreak) _breakTimeLeft = 0f;
        }

        private IEnumerator RunLevel()
        {
            while (_currentWaveIndex < _currentLevel.Waves.Count)
            {
                WaveData wave = _currentLevel.Waves[_currentWaveIndex];
                if (_currentWaveIndex == 0)
                {
                    yield return new WaitForSeconds(wave.DelayBeforeWave);
                }
                else
                {
                    yield return StartCoroutine(RunBreak(wave.DelayBeforeWave));
                }


                EventBus.Invoke<WaveChange>(new WaveChange(_currentWaveIndex + 1, _currentLevel.Waves.Count));
                
                yield return StartCoroutine(SpawnWave(wave));
                
                yield return new WaitUntil(() => _aliveEnemies <= 0 && !_isSpawning);

                GetWaveRewards();
                
                _completedWaves++;
                _currentWaveIndex++;
            }

            int fullClearBonus = _crystalEarnedThisLevel;
            _crystalEarnedThisLevel += fullClearBonus;
            EventBus.Invoke(new CrystalsEarned(fullClearBonus));

            _isRunning = false;
            _currentLevel = null;
            EventBus.Invoke(new AllWavesCompleted());
        }
        
        private IEnumerator RunBreak(float duration)
        {
            _breakTimeLeft = Mathf.Max(0f, duration);
            _isOnBreak = true;
            EventBus.Invoke(new WaveBreakStarted());

            while (_breakTimeLeft > 0f)
            {
                _breakTimeLeft -= Time.deltaTime;
                yield return null;
            }

            EndBreak();
        }

        private void EndBreak()
        {
            if (!_isOnBreak) return;

            _isOnBreak = false;
            _breakTimeLeft = 0f;
            EventBus.Invoke(new WaveBreakFinished());
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
    
    public struct AllWavesCompleted {}
    public struct WaveBreakStarted {}
    public struct WaveBreakFinished {}
}