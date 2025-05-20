using Tetris.TetrisNetworking;
using UnityEngine;

namespace Tetris.MainMenu.Windows
{
    public class MultiplayerWindow : WindowBase
    {
        public const string ASSET_PATH = "MainMenu/MultiplayerWindow";
        
        private new MultiplayerWindowView _view => (MultiplayerWindowView)base._view;

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
            _view.OnBackButtonClick += OnBackButtonClick;
            _view.OnHostButtonClick += OnHostButtonClicked;
            _view.OnJoinButtonClick += OnJoinButtonClick;
        }

        private void UnBindButtons()
        {
            _view.OnBackButtonClick -= OnBackButtonClick;
            _view.OnHostButtonClick -= OnHostButtonClicked;
            _view.OnJoinButtonClick -= OnJoinButtonClick;
        }
        
        private void OnHostButtonClicked()
        {
            GameLobby.Instance.StartHosting("default-name", 2, false);
        }
        
        private void OnJoinButtonClick()
        {
            GameLobby.Instance.StartJoining();
        }

        private void OnBackButtonClick()
        {
            WindowsManager.CreateWindow<MainMenuWindow>(MainMenuWindow.ASSET_PATH, transform.parent);
        }
    }
}