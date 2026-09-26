using System;
using Game.Events;

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
            EventBus.Subscribe<GoldEarned>(AddGold);
            EventBus.Subscribe<CrystalsEarned>(AddCrystals);
        }

        public bool TrySpendGold(int amount)
        {
            if (Gold < amount) return false;
            Gold -= amount;
            OnGoldChanged?.Invoke(Gold);
            return true;
        }
        
        private void AddGold(GoldEarned earned)
        {
            Gold += earned.Amount;
            OnGoldChanged?.Invoke(Gold);
        }

        private void AddCrystals(CrystalsEarned earned)
        {
            CrystalsThisLevel += earned.Amount;
            OnCrystalsChanged?.Invoke(CrystalsThisLevel);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<GoldEarned>(AddGold);
            EventBus.Unsubscribe<CrystalsEarned>(AddCrystals);
        }
    }
    
    public struct GoldEarned
    {
        public int Amount;
        public GoldEarned(int amount) => Amount = amount;
    }

    public struct CrystalsEarned
    {
        public int Amount;
        public CrystalsEarned(int amount) => Amount = amount;
    }
}