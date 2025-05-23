using System;
using Tetris.Gameplay.Core;
using Tetris.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        public event Action OnWantRematch;
        
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private Button _rematchButton;
        [SerializeField] private GameObject _holder;

        private void Start()
        {
            _holder.SetActive(false);
            _rematchButton.onClick.AddListener(OnClickRematch);
            Session.Instance.gameModeSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver(ulong looserTeam)
        {
            _holder.SetActive(true);
            var isMeWinner = PlayerControllerManager.minePlayerController.OwnerClientId != looserTeam;
            _gameOverText.text = isMeWinner ? "YOU WIN!" : "YOU LOSE!";
        }

        private void OnClickRematch()
        {
            _holder.SetActive(false);
            _rematchButton.onClick.RemoveListener(OnClickRematch);
            OnWantRematch?.Invoke();
        }
    }
}