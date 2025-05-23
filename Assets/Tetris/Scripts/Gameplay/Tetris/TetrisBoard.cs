using System.Collections.Generic;
using Tetris.Data;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Tetris
{
    public class TetrisBoard : NetworkBehaviour
    {
        [SerializeField] private Transform _pieceSpawnPoint;
        [SerializeField] private TetrisData _data;
        
        private Transform[,] _grid;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            BoardManager.Instance.ReigsterBoard(OwnerClientId, this);
        }

        private void Awake()
        {
            _grid = new Transform[_data.width, _data.height];
        }
        
        public bool IsInsideGrid(Vector2 pos)
        {
            return pos.x >= 0 && pos.x < _data.width && pos.y >= 0;
        }
        
        public bool IsOccupied(Vector2 pos)
        {
            if (pos.y >= _data.height) return false;
            return _grid[(int)pos.x, (int)pos.y] != null;
        }
        
        public void AddToGrid(Transform block)
        {
            var pos = Round((Vector2)(block.position) - (Vector2)transform.position);
            if (pos.y < _data.height)
                _grid[(int)pos.x, (int)pos.y] = block;
        }
        
        public void ClearFullLines()
        {
            var fullLines = new List<int>();

            for (int y = 0; y < _data.height; y++)
            {
                if (IsLineFull(y))
                {
                    fullLines.Add(y);
                    ClearLine(y);
                }
            }

            if (fullLines.Count == 0)
                return;

            int shift = 0;
            for (int y = 0; y < _data.height; y++)
            {
                if (fullLines.Contains(y))
                {
                    shift++;
                }
                else if (shift > 0)
                {
                    ShiftLineDown(y, shift);
                }
            }
        }

        private bool IsLineFull(int y)
        {
            for (int x = 0; x < _data.width; x++)
            {
                if (_grid[x, y] == null)
                    return false;
            }
            return true;
        }

        private void ClearLine(int y)
        {
            for (int x = 0; x < _data.width; x++)
            {
                if (_grid[x, y] != null)
                {
                    Destroy(_grid[x, y].gameObject);
                    _grid[x, y] = null;
                }
            }
        }

        private void ShiftLineDown(int y, int amount)
        {
            for (int x = 0; x < _data.width; x++)
            {
                var block = _grid[x, y];
                if (block != null)
                {
                    _grid[x, y] = null;
                    _grid[x, y - amount] = block;
                    block.position += Vector3.down * amount;
                }
            }
        }
        
        private Vector2 Round(Vector2 pos)
        {
            return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        }

        public Vector3 GetPieceSpawnPoint()
        {
            return _pieceSpawnPoint.position;
        }
    }
}