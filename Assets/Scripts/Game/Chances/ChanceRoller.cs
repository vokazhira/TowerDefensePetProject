using UnityEngine;

namespace Game.Chances
{
    public class ChanceRoller : IChanceRoller
    {
        public bool Roll(float chancePercent)
        {
            if (chancePercent <= 0f) return false;
            if (chancePercent >= 100f) return true;

            return Random.Range(0f, 100f) < chancePercent;
        }
    }
}