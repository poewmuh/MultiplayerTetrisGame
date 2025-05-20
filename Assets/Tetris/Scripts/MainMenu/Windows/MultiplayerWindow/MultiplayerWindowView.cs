using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.MainMenu.Windows
{
    public class MultiplayerWindowView : WindowViewBase
    {
        public event Action OnHostButtonClick;
        public event Action OnJoinButtonClick;
        public event Action OnBackButtonClick;
        
        [Header("ButtonsLink")]
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _backButton;

        private void Awake()
        {
            _hostButton.onClick.AddListener(OnHostButtonClicked);
            _joinButton.onClick.AddListener(OnJoinButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveListener(OnHostButtonClicked);
            _joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
        
        private void OnHostButtonClicked()
        {
            OnHostButtonClick?.Invoke();
        }
        
        private void OnJoinButtonClicked()
        {
            OnJoinButtonClick?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            OnBackButtonClick?.Invoke();
        }
    }
}