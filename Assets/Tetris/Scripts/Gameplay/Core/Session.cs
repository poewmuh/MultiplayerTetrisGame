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
        public SessionState state => (SessionState)_state.Value;
        public GameModeType gameModeType => _gameModeType;
        
        private readonly NetworkVariable<float> _elapsedTime = new ();
        private readonly NetworkVariable<byte> _state = new ();
        
        [SerializeField] private GameModeType _gameModeType;
        
        protected override void Initialize()
        {
            SetupGameMode();
            SetState(SessionState.Initialized);
            StartGame();
        }

        private void SetupGameMode()
        {
            switch (gameModeType)
            {
                case GameModeType.Singleplayer: GameMode = new SinglePlayerGame(); break;
                case GameModeType.TwoPlayers: GameMode = new TwoPlayersGame(); break;
            }
        }

        private void SetState(SessionState state)
        {
            if (IsServer) _state.Value = (byte)state;
            OnSessionStateChanged?.Invoke(state);
            Debug.Log($"[Session] State changed: {state}");
        }

        private void StartGame()
        {
            GameMode.StartGame();
            SetState(SessionState.Started);
        }

        public void FinishGame()
        {
            SetState(SessionState.Finished);
        }
    }
}