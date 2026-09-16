using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enemies.Factories;
using GameData.Observer;
using Levels;
using UnityEngine;
using Waves.ScriptableObjects;

namespace Waves
{
    public class WaveSystem : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _spawner;
        
        private LevelData _currentLevel;
        private int _currentWaveIndex;
        private int _aliveEnemies;
        private bool _isSpawning;
        private bool _isRunning;

        private void OnEnable()
        {
            GameEvents.OnEnemyDied += OnEnemydied;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyDied -= OnEnemydied;
        }

        public void StartLevel(LevelData level)
        {
            if (_isRunning || level == null || level.Waves.Count == 0) return;
            
            _currentLevel = level;
            _isRunning = true;
            _currentWaveIndex = 0;
            _aliveEnemies = 0;

            StartCoroutine(RunLevel());
        }

        private IEnumerator RunLevel()
        {
            while (_currentWaveIndex < _currentLevel.Waves.Count)
            {
                WaveData wave = _currentLevel.Waves[_currentWaveIndex];
                yield return new WaitForSeconds(wave.DelayBeforeWave);
                
                GameEvents.NotifyWaveStarted();
                yield return StartCoroutine(SpawnWave(wave));
                
                yield return new WaitUntil(() => _aliveEnemies <= 0 && !_isSpawning);
                
                GameEvents.NotifyWaveCompleted();
                _currentWaveIndex++;
            }
            
            GameEvents.NotifyAllWavesCompleted();
            _isRunning = false;
            _currentLevel = null;
        }

        private IEnumerator SpawnWave(WaveData wave)
        {
            _isSpawning = true;

            foreach (WaveContent content in wave.Contents)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    Enemy enemy = _spawner.Spawn(content.EnemyType);
                    _aliveEnemies++;
                    
                    yield return new WaitForSeconds(content.DelayBetweenSpawns);
                }
            }
            
            _isSpawning = false;
        }

        private void OnEnemydied(Enemy enemy)
        {
            _aliveEnemies = Mathf.Max(0, _aliveEnemies - 1);
        }
    }
}