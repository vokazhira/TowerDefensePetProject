using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TotalCrystalsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalCrystalsText;

        private void Start()
        {
            GameSession.Instance.OnTotalCrystalsChanged += Refresh;
            Refresh(GameSession.Instance.TotalCrystals);
        }

        private void OnDestroy()
        {
            if (GameSession.Instance != null)
            {
                GameSession.Instance.OnTotalCrystalsChanged -= Refresh;
            }
        }

        private void Refresh(int crystals)
        {
            _totalCrystalsText.text = crystals.ToString();
        }
    }
}