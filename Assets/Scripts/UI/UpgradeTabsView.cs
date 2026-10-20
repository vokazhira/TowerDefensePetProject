using UnityEngine;

namespace UI
{
    public class UpgradeTabsView : MonoBehaviour
    {
        [SerializeField] private UpgradeTabButton[] _tabs;
        [SerializeField] private UpgradeTabButton _defaultTab;

        private void Awake()
        {
            foreach (UpgradeTabButton tab in _tabs)
            {
                tab.Clicked += Select;
            }

            Select(_defaultTab != null ? _defaultTab : _tabs[0]);
        }

        private void OnDestroy()
        {
            foreach (UpgradeTabButton tab in _tabs)
            {
                if (tab != null) tab.Clicked -= Select;
            }
        }

        public void Select(UpgradeTabButton selected)
        {
            foreach (UpgradeTabButton tab in _tabs)
            {
                tab.SetSelected(tab == selected);
            }
        }
    }
}