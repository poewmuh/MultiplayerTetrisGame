using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.MainMenu.Windows
{
    public class MultiplayerWindowView : WindowViewBase
    {
        [Header("ButtonsLink")]
        [SerializeField] private Button _backButton;

        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            WindowsManager.CreateWindow<MainMenuWindow>(MainMenuWindow.ASSET_PATH, transform.parent);
        }
    }
}