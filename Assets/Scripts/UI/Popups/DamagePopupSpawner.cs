using Game.Events;
using Lean.Pool;
using Towers;
using UnityEngine;

namespace UI.Popups
{
    public class DamagePopupSpawner : MonoBehaviour
    {
        [SerializeField] private Popup _popupPrefab;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, 0f);
        
        private void OnEnable() => EventBus.Subscribe<EnemyDamaged>(HandleEnemyDamaged);
        private void OnDisable() => EventBus.Unsubscribe<EnemyDamaged>(HandleEnemyDamaged);
        
        private void HandleEnemyDamaged(EnemyDamaged e)
        {
            string amount = Mathf.RoundToInt(e.Amount).ToString();
            string text = e.IsCritical ? $"Крит!\n {amount}" : amount;
            Spawn(e.Position, text, Color.red);
        }
        
        private void Spawn(Vector3 position, string text, Color color)
        {
            Popup popup = LeanPool.Spawn(_popupPrefab, position + _offset, Quaternion.identity);
            popup.Show(text, color);
        }
    }
    
    public readonly struct EnemyDamaged
    {
        public Vector3 Position { get; }
        public float Amount { get; }
        public bool IsCritical { get; }

        public EnemyDamaged(Vector3 position, float amount, bool isCritical)
        {
            Position = position;
            Amount = amount;
            IsCritical = isCritical;
        }
    }
}