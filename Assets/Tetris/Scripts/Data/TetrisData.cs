using System.Collections.Generic;
using Tetris.Gameplay.Tetris;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Data
{
    [CreateAssetMenu(fileName = "TetrisData", menuName = "Data/TetrisData")]
    public class TetrisData : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _cellSize;

        [SerializeField] private List<NetworkObject> _standartPiecePrefabs;
        
        [SerializeField] private FallData _fallData;
        
        public int width => _width;
        public int height => _height;
        public int cellSize => _cellSize;

        public List<NetworkObject> GetStandartPiecePrefabs()
        {
            return _standartPiecePrefabs;
        }
        
        public FallData GetFallData() => _fallData;
    }
}