using System;
using Tetris.Assembly;
using Tetris.Gameplay.Core;
using Tetris.Tools;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Tetris.TetrisNetworking
{
    public class GameLobby : MonoSingleton<GameLobby>
    {
        public LobbyUser currentLobbyUser { get; private set; }
        public bool IsConnecting { get; private set; }
        
        private Lobby _joinedLobby;

        private float _heartBitTimer = 5f;
        private float _timer;
        private bool _stopHeartBit;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            TrySignIn();
        }

        private async void TrySignIn()
        {
            if (string.IsNullOrEmpty(Application.cloudProjectId))
            {
                OnSignInFailed();
                return;
            }

            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    InitializationOptions initializationOptions = new InitializationOptions();
                    initializationOptions.SetProfile("Player_" + UnityEngine.Random.Range(0, 10000));

                    await UnityServices.InitializeAsync(initializationOptions);
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    OnSignInComplete();
                }
            }
            catch (Exception)
            {
                OnSignInFailed();
            }
        }
        
        private void OnSignInComplete()
        {
            Debug.Log($"[LobbyManager] SIGN IN COMPLETE ID: {AuthenticationService.Instance.PlayerId}");
            currentLobbyUser = new LobbyUser(AuthenticationService.Instance.PlayerId, "noName");
        }
        
        private void OnSignInFailed()
        {
            Debug.LogError("[LobbyManager] SignIn Failed");
        }

        public async void StartHosting(string sessionName, int maxPlayers, bool isPrivate)
        {
            if (IsConnecting) return;
            
            IsConnecting = true;
            _joinedLobby = await NetworkHelper.CreateLobby(sessionName, maxPlayers, isPrivate);
            IsConnecting = false;
            if (_joinedLobby == null)
            {
                return;
            }
            SceneSwitcher.LoadNetScene(SceneType.Gameplay);
        }

        private void Update()
        {
            //todo : create update lobby class
            if (_stopHeartBit) return;
            if (!NetworkManager.Singleton.IsServer) return;
            if (_joinedLobby == null) return;
            if (!Session.Instance) return;
            if (Session.Instance.gameMode.currentState != GameState.Empty)
            {
                _stopHeartBit = true;
                NetworkHelper.UpdateLobby(_joinedLobby.Id, new UpdateLobbyOptions() { IsLocked = true });
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= _heartBitTimer)
            {
                _timer = 0f;
                NetworkHelper.SendHeartbeatPing(_joinedLobby.Id);
            }
        }

        public async void StartJoining()
        {
            if (IsConnecting) return;
            
            IsConnecting = true;
            _joinedLobby = await NetworkHelper.QuickJoinLobby();
            IsConnecting = false;
            if (_joinedLobby == null) return;
            SceneSwitcher.LoadNetScene(SceneType.Gameplay);
        }
    }
}