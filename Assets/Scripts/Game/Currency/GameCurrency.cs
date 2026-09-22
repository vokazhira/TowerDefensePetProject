using System;
using Game.Events.Observer;

namespace Game.Currency
{
    public class GameCurrency : IDisposable
    {
        public int Gold { get; private set; }
        public int CrystalsThisLevel { get; private set; }
        
        public event Action<int> OnGoldChanged;
        public event Action<int> OnCrystalsChanged;

        public GameCurrency()
        {
            GameEvents.OnGoldEarned += AddGold;
            GameEvents.OnCrystalsEarned += AddCrystals;
        }

        public bool TrySpendGold(int amount)
        {
            if (Gold < amount) return false;
            Gold -= amount;
            OnGoldChanged?.Invoke(Gold);
            return true;
        }
        
        private void AddGold(int amount)
        {
            Gold += amount;
            OnGoldChanged?.Invoke(Gold);
        }

        private void AddCrystals(int amount)
        {
            CrystalsThisLevel += amount;
            OnCrystalsChanged?.Invoke(CrystalsThisLevel);
        }

        public void Dispose()
        {
            GameEvents.OnGoldEarned -= AddGold;
            GameEvents.OnCrystalsEarned -= AddCrystals;
        }
    }
}