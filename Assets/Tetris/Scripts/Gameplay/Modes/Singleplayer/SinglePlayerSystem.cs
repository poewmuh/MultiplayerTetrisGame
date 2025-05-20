using System;
using Cysharp.Threading.Tasks;
using Tetris.Gameplay.Datas;
using Unity.Netcode;

namespace Tetris.Gameplay.Core
{
    public class SinglePlayerSystem : GameplaySystem<SinglePlayerSystem>, IGameModeSystem
    {
        public event Action OnTeamWin;
        public event Action OnGameOver;
        
        public SinglePlayerData Data { get; private set; }
        public GameState state => (GameState)_state.Value;

        private readonly NetworkVariable<byte> _state = new ();
        private bool _isGameStarted;

        protected override void Initialize()
        {
            WaitGameStart().Forget();
        }

        private async UniTaskVoid WaitGameStart()
        {
            await UniTask.WaitWhile(() => !Session.Instance && Session.Instance.state != SessionState.Started);
            await UniTask.WaitWhile(() => Session.Instance.GameMode.currentState != GameState.InGame);
            _isGameStarted = true;
        }
    }
}