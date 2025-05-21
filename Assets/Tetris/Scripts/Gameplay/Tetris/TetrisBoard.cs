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
        
        public void ClearFullRows()
        {
            
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