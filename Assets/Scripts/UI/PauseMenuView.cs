using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _pausePanel.SetActive(false);
            _pauseButton.onClick.AddListener(Pause);
            _continueButton.onClick.AddListener(Resume);
            _exitButton.onClick.AddListener(ExitToMenu);
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            _pausePanel.SetActive(true);
        }

        private void Resume()
        {
            Time.timeScale = 1f;
            _pausePanel.SetActive(false);
        }
        
        private void ExitToMenu() => GameSession.Instance.ReturnToMenu();

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(Pause);
            _continueButton.onClick.RemoveListener(Resume);
            _exitButton.onClick.RemoveListener(ExitToMenu);
        }
    }
}