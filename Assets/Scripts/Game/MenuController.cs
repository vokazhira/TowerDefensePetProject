using ScriptableObjectData.LevelSO;
using UI;
using UnityEngine;

namespace Game
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private UpgradePanelView _upgradePanelView;

        private void Start()
        {
            _upgradePanelView.Init();
        }
        
        public void StartLevel(LevelData levelData)
        {
            GameSession.Instance.OpenLevel(levelData);
        }

        public void OpenUpgrades() => _upgradePanel.SetActive(true);
        public void CloseUpgrades() => _upgradePanel.SetActive(false);
    }
}