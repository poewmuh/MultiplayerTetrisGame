using System;
using Cysharp.Threading.Tasks;
using Tetris.Data;
using Tetris.Gameplay.Datas;
using Tetris.Gameplay.Tetris;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Core
{
    public class TwoPlayersSystem : GameplaySystem<TwoPlayersSystem>, IGameModeSystem
    {
        public event Action<ulong> OnGameOver;

        public TwoPlayersData Data { get; private set; }
        
        [SerializeField] private TetrisData _data;

        private FallHandler _fallHandler;
        private bool _isGameStarted;
        private NetworkVariable<float> _currentFallTime = new();

        protected override void Initialize()
        {
            if (!IsServer) return;
            _fallHandler = new FallHandler(_data.GetFallData());
            _fallHandler.OnFallTimeChanged += OnFallTimeChanged;
            WaitGameStart().Forget();
        }

        protected override void OnDestroy()
        {
            _fallHandler.OnFallTimeChanged -= OnFallTimeChanged;
            _fallHandler = null;
            base.OnDestroy();
        }

        private void OnFallTimeChanged(float newFallTime)
        {
            if (!Mathf.Approximately(newFallTime, _currentFallTime.Value))
            {
                _currentFallTime.Value = newFallTime;
            }
        }

        private async UniTaskVoid WaitGameStart()
        {
            await UniTask.WaitWhile(() => !Session.Instance && Session.Instance.state != SessionState.Started);
            await UniTask.WaitWhile(() => Session.Instance.gameMode.currentState != GameState.InGame);
            _fallHandler.SetPauseState(false);
            _isGameStarted = true;
            foreach (var tetrisBoard in BoardManager.Instance.GetAllBoards())
            {
                SpawnPiece(tetrisBoard.OwnerClientId);
            }
        }
        
        public void SpawnPiece(ulong ownerId)
        {
            if (!IsServer)
            {
                ResponseSpawnRpc(ownerId);
                return;
            }
            var standartPiecePrefabs = _data.GetStandartPiecePrefabs();
            var pieceToSpawn = standartPiecePrefabs[UnityEngine.Random.Range(0, standartPiecePrefabs.Count)];
            var board = BoardManager.Instance.GetBoardByUserId(ownerId);
            var piece = Instantiate(pieceToSpawn, board.GetPieceSpawnPoint(), Quaternion.identity);
            piece.SpawnWithOwnership(ownerId);
            OnPieceSpawnedRpc(piece.NetworkObjectId);
        }

        private void GameOver(ulong looserId)
        {
            _fallHandler.SetPauseState(true);
            _fallHandler.ResetFallTime();
            OnGameOver?.Invoke(looserId);
        }

        [Rpc(SendTo.Everyone)]
        private void OnPieceSpawnedRpc(ulong pieceNetId)
        {
            var piece = NetworkManager.SpawnManager.SpawnedObjects[pieceNetId];
            var board = BoardManager.Instance.GetBoardByUserId(piece.OwnerClientId);
            var tetrisPiece = piece.GetComponent<TetrisPiece>();
            tetrisPiece.SetBoard(board);
            
            if (!tetrisPiece.IsValidPosition(Vector2.zero))
            {
                GameOver(piece.OwnerClientId);
                piece.Despawn();
                return;
            }
            
            tetrisPiece.Init();
        }

        [Rpc(SendTo.Server)]
        private void ResponseSpawnRpc(ulong boardOwnerId)
        {
            SpawnPiece(boardOwnerId);
        }
        
        public float GetFallDelay()
        {
            return _currentFallTime.Value;
        }

        private void Update()
        {
            if (!IsServer) return;
            _fallHandler.Tick(Time.deltaTime);
        }
    }
}