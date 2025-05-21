using System;
using Cysharp.Threading.Tasks;
using Tetris.Data;
using Tetris.Gameplay.Datas;
using Tetris.Gameplay.Tetris;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

namespace Tetris.Gameplay.Core
{
    public class SinglePlayerSystem : GameplaySystem<SinglePlayerSystem>, IGameModeSystem
    {
        public event Action OnGameOver;

        public SinglePlayerData Data { get; private set; }

        [SerializeField] private TetrisData _data;
        [SerializeField] private Transform _boardSpawnPoint;
        [SerializeField] private TetrisBoard _boardPrefab;


        private TetrisBoard _gameBoard;
        private bool _isGameStarted;

        protected override void Initialize()
        {
            _gameBoard = Instantiate(_boardPrefab, _boardSpawnPoint.position, Quaternion.identity);
            WaitGameStart().Forget();
        }

        private async UniTaskVoid WaitGameStart()
        {
            await UniTask.WaitWhile(() => !Session.Instance && Session.Instance.state != SessionState.Started);
            await UniTask.WaitWhile(() => Session.Instance.GameMode.currentState != GameState.InGame);
            _isGameStarted = true;
            SpawnPiece(0);
        }
        
        public void SpawnPiece(ulong ownerId)
        {
            var standartPiecePrefabs = _data.GetStandartPiecePrefabs();
            var piece = Instantiate(standartPiecePrefabs[UnityEngine.Random.Range(0, standartPiecePrefabs.Count)], _gameBoard.GetPieceSpawnPoint(), Quaternion.identity);
            piece.transform.SetParent(_gameBoard.transform);
            piece.GetComponent<TetrisPiece>().Init(_gameBoard);
        }

        public float GetFallDelay()
        {
            return 0.7f;
        }
    }
}