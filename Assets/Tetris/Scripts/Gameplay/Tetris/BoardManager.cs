using System.Collections.Generic;
using System.Linq;
using Tetris.Gameplay.Core;
using Tetris.Gameplay.Player;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Tetris
{
    public class BoardManager : GameplaySystem<BoardManager>
    {
        [SerializeField] private NetworkObject _boardPrefab;
        [SerializeField] private List<Transform> _boardSpawnPoints;

        private readonly Dictionary<ulong, TetrisBoard> _playerBoards = new();
        
        protected override void Initialize()
        {
            if (!IsServer) return;
            
            foreach (var playerController in PlayerControllerManager.Instance.GetPlayersController())
            {
                SpawnBoardOnFreePlace(playerController);
            }

            PlayerControllerManager.Instance.OnPlayerControllerReceived += OnNewPlayerReceived;
        }
        
        private void OnNewPlayerReceived(PlayerController player)
        {
            SpawnBoardOnFreePlace(player);
            if (PlayerControllerManager.Instance.GetPlayersController().Count == Session.Instance.GameMode.playersCount)
            {
                PlayerControllerManager.Instance.OnPlayerControllerReceived -= OnNewPlayerReceived;
            }
        }

        private void SpawnBoardOnFreePlace(PlayerController player)
        {
           SpawnBoard(player);
        }
        
        private void SpawnBoard(PlayerController player)
        {
            Debug.Log($"Spawning board for clientId: {player.OwnerClientId}");
            var spawnPoint = _boardSpawnPoints[0];
            var board = Instantiate(_boardPrefab, spawnPoint.position, Quaternion.identity);
            board.SpawnWithOwnership(player.OwnerClientId, true);
            _boardSpawnPoints.RemoveAt(0);
        }

        public void ReigsterBoard(ulong ownerId, TetrisBoard board)
        {
            _playerBoards.Add(ownerId, board);
        }

        public TetrisBoard GetBoardByUserId(ulong userId)
        {
            return _playerBoards[userId];
        }

        public List<TetrisBoard> GetAllBoards()
        {
            return _playerBoards.Values.ToList();
        }
    }
}