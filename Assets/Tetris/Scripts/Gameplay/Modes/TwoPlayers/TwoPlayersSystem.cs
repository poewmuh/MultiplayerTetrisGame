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
        public event Action OnGameOver;

        public TwoPlayersData Data { get; private set; }
        
        [SerializeField] private TetrisData _data;
        
        private bool _isGameStarted;

        protected override void Initialize()
        {
            if (!IsServer) return;
            WaitGameStart().Forget();
        }

        private async UniTaskVoid WaitGameStart()
        {
            await UniTask.WaitWhile(() => !Session.Instance && Session.Instance.state != SessionState.Started);
            await UniTask.WaitWhile(() => Session.Instance.GameMode.currentState != GameState.InGame);
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
            Debug.Log("Spawning piece");
            var standartPiecePrefabs = _data.GetStandartPiecePrefabs();
            var pieceToSpawn = standartPiecePrefabs[UnityEngine.Random.Range(0, standartPiecePrefabs.Count)].GetComponent<NetworkObject>();
            var board = BoardManager.Instance.GetBoardByUserId(ownerId);
            var piece = Instantiate(pieceToSpawn, board.GetPieceSpawnPoint(), Quaternion.identity);
            piece.SpawnWithOwnership(ownerId);
            OnPieceSpawnedRpc(piece.NetworkObjectId);
            
        }

        [Rpc(SendTo.Everyone)]
        private void OnPieceSpawnedRpc(ulong pieceNetId)
        {
            var piece = NetworkManager.SpawnManager.SpawnedObjects[pieceNetId];
            var board = BoardManager.Instance.GetBoardByUserId(piece.OwnerClientId);
            if (piece.IsOwner)
            {
                piece.transform.SetParent(board.transform);
            }

            piece.GetComponent<TetrisPiece>().Init(board);
        }

        [Rpc(SendTo.Server)]
        private void ResponseSpawnRpc(ulong boardOwnerId)
        {
            SpawnPiece(boardOwnerId);
        }
        
        public float GetFallDelay()
        {
            return 0.7f;
        }
    }
}