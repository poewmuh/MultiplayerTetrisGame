using UnityEngine;

namespace Tetris.MainMenu.Windows
{
    public class MainMenuWindow : WindowBase
    {
        public const string ASSET_PATH = "MainMenu/MainMenuWindow";

        private new MainMenuWindowView _view => (MainMenuWindowView)base._view;

        public override void StartOpen()
        {
            BindButtons();
            base.StartOpen();
        }

        public override void StartClose()
        {
            UnBindButtons();
            base.StartClose();
        }

        private void BindButtons()
        {
            _view.OnSinglePlayerClick += OnSingleplayerClick;
            _view.OnMultiPlayerClick += OnMultiplayerClick;
            _view.OnSettingsClick += OnSettingsClick;
            _view.OnExitClick += OnExitClick;
        }

        private void UnBindButtons()
        {
            _view.OnSinglePlayerClick -= OnSingleplayerClick;
            _view.OnMultiPlayerClick -= OnMultiplayerClick;
            _view.OnSettingsClick -= OnSettingsClick;
            _view.OnExitClick -= OnExitClick;
        }
        
        private void OnSingleplayerClick()
        {
            Debug.Log("[Menu Buttons] OnSingleplayerClick");
        }
        
        private void OnMultiplayerClick()
        {
            WindowsManager.CreateWindow<MultiplayerWindow>(MultiplayerWindow.ASSET_PATH, transform.parent);
        }
        
        private void OnSettingsClick()
        {
            Debug.Log("[Menu Buttons] OnSettingsClick");
        }

        private void OnExitClick()
        {
            Debug.Log("[Menu Buttons] OnExitButtonClicked");
            Application.Quit();
        }
    }
}