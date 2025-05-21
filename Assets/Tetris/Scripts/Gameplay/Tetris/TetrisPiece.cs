using Tetris.Gameplay.Core;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Tetris
{
    public class TetrisPiece : NetworkBehaviour
    {
        private TetrisBoard _board;
        private float _fallTimer;
        
        public void Init(TetrisBoard board)
        {
            _board = board;
            if (IsOwner)
            {
                SubscribeInput();
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
            if (!IsOwner) return;
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
                UnSubscribeInput();
                FixToGrid();
                enabled = false;
                Session.Instance.gameModeSystem.SpawnPiece(_board.OwnerClientId);
            }
        }
        
        private void FixToGrid()
        {
            foreach (Transform block in transform)
            {
                _board.AddToGrid(block);
            }
        }

        private bool TryMove(Vector3 direction)
        {
            if (IsValidPosition(direction))
            {
                transform.position += direction;
                return true;
            }
            return false;
        }
        
        private bool IsValidPosition(Vector3 offset)
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
            TryRotate();
        }
        
        public bool TryRotate()
        {
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
    }
}