using System;
using System.Collections.Generic;
using Game.Events.Observer;
using Game.States;
using ScriptableObjectData.LevelSO;
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
        public int TotalCrystals { get; private set; }
        
        public event Action<int> OnTotalCrystalsChanged;
        public event Action<TowerStatType> OnPermanentUpgradeChanged;
        
        private Dictionary<TowerStatType, int> _permanentLevels = new Dictionary<TowerStatType, int>();
         
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

            GameEvents.OnCrystalsEarned += AddCrystals;
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
            SceneManager.LoadScene("Game");
        }

        public void ReturnToMenu()
        {
            State = GameState.Menu;
            SceneManager.LoadScene("Menu");
        }

        private void AddCrystals(int amount)
        {
            TotalCrystals += amount;
            OnTotalCrystalsChanged?.Invoke(TotalCrystals);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                GameEvents.OnCrystalsEarned -= AddCrystals;
                Instance = null;
            }
        }
    }
}