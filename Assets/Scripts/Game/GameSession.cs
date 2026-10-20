using System;
using System.Collections.Generic;
using Game.States;
using ScriptableObjectData.LevelSO;
using ScriptableObjectData.TowerSO;
using Towers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession Instance;

        public GameState State { get; private set; } = GameState.Menu;
        public LevelData SelectedLevel { get; private set; }
        public TowerDataSO SelectedTower { get; private set; }
        public int TotalCrystals { get; private set; }
        
        public event Action<int> OnTotalCrystalsChanged;
        public event Action<TowerStatType> OnPermanentUpgradeChanged;
        
        private Dictionary<TowerStatType, int> _permanentLevels = new Dictionary<TowerStatType, int>();
        private HashSet<LevelData> _completedLevels = new HashSet<LevelData>();
         
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            foreach (TowerStatType statType in Enum.GetValues(typeof(TowerStatType)))
            {
                _permanentLevels[statType] = 0;
            }
        }

        public int GetPermanentLevel(TowerStatType statType)
        {
            return _permanentLevels[statType];
        }

        public void IncreasePermanentLevel(TowerStatType statType)
        {
            _permanentLevels[statType]++;
            OnPermanentUpgradeChanged?.Invoke(statType);
        }

        public void AddCrystals(int amount)
        {
            if (amount <= 0) return;

            TotalCrystals += amount;
            OnTotalCrystalsChanged?.Invoke(TotalCrystals);
        }
        
        public bool TrySpendCrystals(int amount)
        {
            if (TotalCrystals < amount) return false;
            
            TotalCrystals -= amount;
            OnTotalCrystalsChanged?.Invoke(TotalCrystals);
            return true;
        }

        public void OpenLevel(LevelData level)
        {
            SelectedLevel = level;
            State = GameState.Game;
            Time.timeScale = 1f;
            SceneManager.LoadScene("Game");
        }
        
        public void SelectLevel(LevelData level)
        {
            SelectedLevel = level;
        }

        public void CompleteSelectedLevel()
        {
            if (SelectedLevel == null) return;

            _completedLevels.Add(SelectedLevel);
        }

        public bool IsLevelCompleted(LevelData level)
        {
            return _completedLevels.Contains(level);
        }

        public void ReturnToMenu()
        {
            State = GameState.Menu;
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
        
        public void SelectTower(TowerDataSO tower)
        {
            SelectedTower = tower;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}