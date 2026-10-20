using Game.Events;
using Game.Waves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NextWaveButtonView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private string _format = "Старт следующей волны\n{0}";
        
        private IWaveBreak _waveBreak;

        public void Init(IWaveBreak waveBreak)
        {
            _waveBreak = waveBreak;
            _button.onClick.RemoveListener(OnClick);
            _button.onClick.AddListener(OnClick);
            Hide();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<WaveBreakStarted>(Show);
            EventBus.Subscribe<WaveBreakFinished>(Hide);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<WaveBreakStarted>(Show);
            EventBus.Unsubscribe<WaveBreakFinished>(Hide);
        }

        private void Update()
        {
            if (_waveBreak == null || !_root.activeSelf) return;

            _label.text = string.Format(_format, Mathf.CeilToInt(_waveBreak.BreakTimeLeft));
        }

        private void Show() => _root.SetActive(true);
        private void Hide() => _root.SetActive(false);

        private void OnClick() => _waveBreak?.SkipBreak();

        private void OnDestroy() => _button.onClick.RemoveListener(OnClick);
    }
}