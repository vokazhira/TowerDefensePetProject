using Game.Events;
using Lean.Pool;
using Towers;
using UnityEngine;

namespace UI.Popups
{
    public class UpgradePopupSpawner : MonoBehaviour
    {
        [SerializeField] private Popup _popupPrefab;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Color _upgradeColor = Color.green;
        [SerializeField] private Color _freeUpgradeColor = Color.cyan;

        private void OnEnable() => EventBus.Subscribe<StatUpgraded>(HandleStatUpgraded);
        private void OnDisable() => EventBus.Unsubscribe<StatUpgraded>(HandleStatUpgraded);

        private void HandleStatUpgraded(StatUpgraded upgraded)
        {
            string value = "+" + TowerStatRules.Format(upgraded.StatType, upgraded.Amount);
            string text = upgraded.IsFree ? $"{value}\nБесплатно!" : value;
            Color color = upgraded.IsFree ? _freeUpgradeColor : _upgradeColor;

            Popup popup = LeanPool.Spawn(_popupPrefab, _container);
            popup.transform.position = upgraded.Position;
            popup.transform.SetAsLastSibling();
            popup.Show(text, color);
        }
    }
    
    public readonly struct StatUpgraded
    {
        public TowerStatType StatType { get; }
        public float Amount { get; }
        public bool IsFree { get; }
        public Vector3 Position { get; }

        public StatUpgraded(TowerStatType statType, float amount, bool isFree, Vector3 position)
        {
            StatType = statType;
            Amount = amount;
            IsFree = isFree;
            Position = position;
        }
    }
}