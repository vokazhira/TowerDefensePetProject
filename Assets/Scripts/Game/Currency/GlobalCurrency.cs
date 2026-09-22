/*using System;
using Game.Events.Observer;

namespace Game.Currency
{
    public class GlobalCurrency : IDisposable
    {
        private int _crystals;
        
        public int Crystals => _crystals;
        public event Action<int> OnCrystalsChanged;

        public GlobalCurrency()
        {
            GameEvents.OnCrystalsEarned += HandleCrystalsChanged;
        }
        
        private void HandleCrystalsChanged(int amount)
        {
            _crystals += amount;
            OnCrystalsChanged?.Invoke(_crystals);
        }

        public void Dispose()
        {
            GameEvents.OnCrystalsEarned -= HandleCrystalsChanged;
        }
    }
}*/