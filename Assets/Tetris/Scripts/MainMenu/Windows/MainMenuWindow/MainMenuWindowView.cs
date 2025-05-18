using System;
using Tetris.MainMenu.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.MainMenu
{
    public class MainMenuWindowView : WindowViewBase
    {
        public event Action OnSinglePlayerClick;
        public event Action OnMultiPlayerClick;
        public event Action OnSettingsClick;
        public event Action OnExitClick;
        
        [Header("ButtonsLink")]
        [SerializeField] private Button _singlePlayerButton;
        [SerializeField] private Button _multiPlayerButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _singlePlayerButton.onClick.AddListener(OnSinglePlayerButtonClicked);
            _multiPlayerButton.onClick.AddListener(OnMultiPlayerButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDestroy()
        {
            _singlePlayerButton.onClick.RemoveListener(OnSinglePlayerButtonClicked);
            _multiPlayerButton.onClick.RemoveListener(OnMultiPlayerButtonClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnSinglePlayerButtonClicked()
        {
            OnSinglePlayerClick?.Invoke();
        }
        
        private void OnMultiPlayerButtonClicked()
        {
            OnMultiPlayerClick?.Invoke();
        }
        
        private void OnSettingsButtonClicked()
        {
            OnSettingsClick?.Invoke();
        }
        
        private void OnExitButtonClicked()
        {
            OnExitClick?.Invoke();
        }
    }
}