using System.Collections.Generic;
using Tetris.Gameplay.Tetris;
using UnityEngine;

namespace Tetris.Data
{
    [CreateAssetMenu(fileName = "TetrisData", menuName = "Data/TetrisData")]
    public class TetrisData : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _cellSize;

        [SerializeField] private List<GameObject> _standartPiecePrefabs;
        
        public int width => _width;
        public int height => _height;
        public int cellSize => _cellSize;

        public List<GameObject> GetStandartPiecePrefabs()
        {
            return _standartPiecePrefabs;
        }
    }
}