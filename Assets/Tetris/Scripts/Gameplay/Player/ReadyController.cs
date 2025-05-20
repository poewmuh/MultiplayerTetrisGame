using System;
using Tetris.Gameplay.Core;
using Tetris.Gameplay.UI;
using Tetris.Tools;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Player
{
    public class ReadyController : NetworkBehaviour
    {
        private ComponentResolver<PlayerHUD> _playerHUD = new();

        private void Start()
        {
            if (!IsOwner) return;
            
            if (_playerHUD.Component == null)
            {
                _playerHUD.OnComponentRegistered += OnHUDRegistered;
                return;
            }

            SubscribeOnHUD();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnSubscribeOnHUD();
        }

        private void OnHUDRegistered()
        {
            _playerHUD.OnComponentRegistered -= OnHUDRegistered;
            SubscribeOnHUD();
        }

        private void SubscribeOnHUD()
        {
            _playerHUD.Component.readyChecker.OnPlayerReady += OnPlayerReady;
        }
        
        private void UnSubscribeOnHUD()
        {
            if (_playerHUD.Component)
            {
                _playerHUD.Component.readyChecker.OnPlayerReady -= OnPlayerReady;
            }
        }

        private void OnPlayerReady()
        {
            Session.Instance.OnSessionStateChanged += OnSessionStateChange;
            SendReadyRpc();
        }

        private void OnSessionStateChange(SessionState state)
        {
            if (state == SessionState.Started)
            {
                Session.Instance.OnSessionStateChanged -= OnSessionStateChange;
                _playerHUD.Component.readyChecker.Hide();
            }
        }

        [Rpc(SendTo.Everyone)]
        private void SendReadyRpc()
        {
            if (IsServer)
            {
                Session.Instance.OnPlayerReady();
            }
        }
    }
}