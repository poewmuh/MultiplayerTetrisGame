using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.MainMenu
{
    public class MenuButtonsController : MonoBehaviour
    {
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
            
        }
        
        private void OnMultiPlayerButtonClicked()
        {
            
        }
        
        private void OnSettingsButtonClicked()
        {
            
        }
        
        private void OnExitButtonClicked()
        {
            Debug.Log("[Menu Buttons] OnExitButtonClicked");
            Application.Quit();
        }
    }
}