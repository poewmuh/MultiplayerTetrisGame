using Tetris.Gameplay.Core;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Tetris
{
    public class TetrisPiece : NetworkBehaviour
    {
        private TetrisBoard _board;
        private float _fallTimer;
        private bool _isPaused = true;

        public void Init()
        {
            Subscribe();
            _isPaused = false;
        }
        
        public void SetBoard(TetrisBoard board)
        {
            _board = board;
            if (IsOwner)
            {
                transform.SetParent(_board.transform);
            }
        }

        private void Subscribe()
        {
            if (IsOwner)
            {
                SubscribeInput();
                Session.Instance.gameMode.OnStateChanged += OnGameModeStateChange;
            }
        }

        private void UnSubscribe()
        {
            if (!IsOwner) return;
            Session.Instance.gameMode.OnStateChanged -= OnGameModeStateChange;
            UnSubscribeInput();
        }

        private void OnGameModeStateChange(GameState state)
        {
            if (state == GameState.GameOver)
            {
                UnSubscribe();
                _isPaused = true;
            }
        }

        private void SubscribeInput()
        {
            TetrisInputController.Instance.OnMoveDown += MoveDown;
            TetrisInputController.Instance.OnMoveLeft += MoveLeft;
            TetrisInputController.Instance.OnMoveRight += MoveRight;
            TetrisInputController.Instance.OnRotate += Rotate;
        }

        private void UnSubscribeInput()
        {
            TetrisInputController.Instance.OnMoveDown -= MoveDown;
            TetrisInputController.Instance.OnMoveLeft -= MoveLeft;
            TetrisInputController.Instance.OnMoveRight -= MoveRight;
            TetrisInputController.Instance.OnRotate -= Rotate;
        }

        private void Update()
        {
            if (!IsOwner || _isPaused) return;
            
            _fallTimer += Time.deltaTime;
            if (_fallTimer >= Session.Instance.gameModeSystem.GetFallDelay())
            {
                _fallTimer = 0;
                TryMoveDown();
            }
        }

        public void MoveLeft() => TryMove(Vector3.left);
        public void MoveRight() => TryMove(Vector3.right);

        public void MoveDown() => TryMoveDown();
        
        private void TryMoveDown()
        {
            if (!TryMove(Vector3.down))
            {
                UnSubscribe();
                FixToGrid();
            }
        }
        
        private void FixToGrid()
        {
            SendOnPieceFixedRpc();
            Session.Instance.gameModeSystem.SpawnPiece(_board.OwnerClientId);
        }

        private bool TryMove(Vector3 direction)
        {
            if (IsValidPosition(direction))
            {
                transform.position += direction;
                SendPositionRpc(transform.position);
                return true;
            }
            return false;
        }
        
        public bool IsValidPosition(Vector3 offset)
        {
            foreach (Transform block in transform)
            {
                var futurePos = Round((Vector2)(block.position + offset) - (Vector2)_board.transform.position);
                if (!_board.IsInsideGrid(futurePos) || _board.IsOccupied(futurePos))
                    return false;
            }
            return true;
        }

        public void Rotate()
        {
            if (TryRotate())
            {
                SendRotationRpc(transform.eulerAngles.z);
            }
        }
        
        private bool TryRotate()
        {
            //todo : fix later
            transform.Rotate(0, 0, -90);

            if (IsValidPosition(Vector3.zero)) return true;
            if (TryMove(Vector3.right) || TryMove(Vector3.left) ||
                TryMove(Vector3.right * 2) || TryMove(Vector3.left * 2))
            {
                return true;
            }
            
            transform.Rotate(0, 0, 90);
            return false;
        }
        
        private Vector2 Round(Vector2 pos)
        {
            return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        }

        [Rpc(SendTo.NotMe)]
        private void SendPositionRpc(Vector2 pos)
        {
            transform.position = pos;
        }

        [Rpc(SendTo.NotMe)]
        private void SendRotationRpc(float rotationZ)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }

        [Rpc(SendTo.Everyone)]
        private void SendOnPieceFixedRpc()
        {
            foreach (Transform block in transform)
            {
                _board.AddToGrid(block);
            }
            
            _board.ClearFullLines();
            
            enabled = false;
            
            if (IsServer)
            {
                NetworkObject.ChangeOwnership(NetworkManager.ServerClientId);
            }
        }
    }
}