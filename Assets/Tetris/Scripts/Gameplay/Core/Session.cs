using System;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Core
{
    public class Session : GameplaySystem<Session>
    {
        public event Action<SessionState> OnSessionStateChanged;
        
        public GameMode GameMode { get; private set; }
        

        public IGameModeSystem gameModeSystem
        {
            get
            {
                if (gameModeType == GameModeType.Singleplayer) return SinglePlayerSystem.Instance;
                if (gameModeType == GameModeType.TwoPlayers) return TwoPlayersSystem.Instance;
                return null;
            }
        }
        
        public float elapsedTime => _elapsedTime.Value;
        public SessionState state { get; private set; } = SessionState.Created;
        public GameModeType gameModeType => _gameModeType;
        
        private readonly NetworkVariable<float> _elapsedTime = new ();

        private int _readyPlayers;
        
        [SerializeField] private GameModeType _gameModeType;
        
        protected override void Initialize()
        {
            SetupGameMode();
            if (!IsServer) return;
            
            SetState(SessionState.Initialized);
        }

        private void SetupGameMode()
        {
            switch (gameModeType)
            {
                case GameModeType.Singleplayer: GameMode = new SinglePlayerGame(); break;
                case GameModeType.TwoPlayers: GameMode = new TwoPlayersGame(); break;
            }
        }

        private void SetState(SessionState newState)
        {
            SendStateChangeRpc((byte)newState);
        }

        [Rpc(SendTo.Everyone)]
        private void SendStateChangeRpc(byte newState)
        {
            state = (SessionState)newState;
            OnSessionStateChanged?.Invoke(state);
            Debug.Log($"[Session] State changed: {state}");

            if (state == SessionState.Started)
            {
                GameMode.StartGame();
            }
        }

        public void FinishGame()
        {
            SetState(SessionState.Finished);
        }

        public void OnPlayerReady()
        {
            _readyPlayers++;
            if (_readyPlayers == GameMode.playersCount)
            {
                SetState(SessionState.Started);
            }
        }
    }
}