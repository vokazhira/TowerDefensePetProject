using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace UI
{
    public class StatUpgradeButton : MonoBehaviour
    {
        [SerializeField] private TowerStatType _statType;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        [SerializeField] private Color _availableColor = Color.blue;
        [SerializeField] private Color _unavailableColor = Color.gray;

        private UpgradeSystem _upgradeSystem;

        public void Init(UpgradeSystem upgradeSystem)
        {
            _upgradeSystem = upgradeSystem;
            _button.onClick.AddListener(OnClick);
            Refresh();
        }

        public void Refresh()
        {
            _nameText.text = GetName(_statType);
            _priceText.text = $"Цена: {_upgradeSystem.GetPrice(_statType)}";
            _valueText.text = _upgradeSystem.GetValueText(_statType);
            
            bool canBuy = _upgradeSystem.CanBuy(_statType);
            _button.interactable = canBuy;
            _background.color = canBuy ? _availableColor : _unavailableColor;
        }

        private void OnClick()
        {
            _upgradeSystem.TryUpgrade(_statType);
            Refresh();
        }

        private string GetName(TowerStatType statType)
        {
            return statType switch
            {
                TowerStatType.Damage => "Урон",
                TowerStatType.AttackSpeed => "Скорость\nатаки",
                TowerStatType.Range => "Дальность",
                TowerStatType.MultishotChance => "Мульти-\nвыстрел",
                TowerStatType.CritChance => "Крит.\nшанс",
                TowerStatType.CritMultiplier => "Крит.\nмножитель",
                TowerStatType.MaxHealth => "Здоровье",
                TowerStatType.HealthRegeneration => "Реген.\nздоровья",
                TowerStatType.Defense => "Защита",
                TowerStatType.VampirismChance => "Шанс\nвампиризма",
                TowerStatType.VampirismMultiplier => "Вампиризм",
                TowerStatType.WaveGoldReward => "Золота за\nволну",
                TowerStatType.WaveCrystalReward => "Кристаллы за\nволну",
                TowerStatType.GoldPerKillBonus => "Золото за\nубийство",
                TowerStatType.FreeUpgradeChance => "Бесплат.\nулучшения",
                _ => statType.ToString()
            };
        }
    }

}