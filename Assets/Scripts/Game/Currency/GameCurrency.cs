using System;
using Game.Events.Observer;

namespace Game.Currency
{
    public class GameCurrency : IDisposable
    {
        private int _gold;
        private int _crystalsThisLevel;
        
        public int Gold => _gold;
        public int CrystalsThisLevel => _crystalsThisLevel;
        
        public event Action<int> OnGoldChanged;
        public event Action<int> OnCrystalsChanged;

        public GameCurrency()
        {
            GameEvents.OnGoldEarned += HandleGoldEarned;
            GameEvents.OnCrystalsEarned += HandleCrystalChanged;
        }

        private void HandleGoldEarned(int amount)
        {
            _gold += amount;
            OnGoldChanged?.Invoke(_gold);
        }

        private void HandleCrystalChanged(int amount)
        {
            _crystalsThisLevel += amount;
            OnCrystalsChanged?.Invoke(_crystalsThisLevel);
        }
        
        public void Reset()
        {
            _gold = 0;
            _crystalsThisLevel = 0;
            OnGoldChanged?.Invoke(_gold);
            OnCrystalsChanged?.Invoke(_crystalsThisLevel);
        }

        public void Dispose()
        {
            GameEvents.OnGoldEarned -= HandleGoldEarned;
            GameEvents.OnCrystalsEarned -= HandleCrystalChanged;
        }
    }
}