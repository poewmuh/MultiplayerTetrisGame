using Tetris.Tools;
using TMPro;
using UnityEngine;

namespace Tetris.Gameplay.UI
{
    public class PlayerHUD : SharedBehaviour
    {
        [SerializeField] private ReadyChecker _readyChecker;
        //[SerializeField] private PingChecker _pingChecker;
        //[SerializeField] private FPSChecker _fpsChecker;
        [SerializeField] private GameOverWindow _gameOverWindow;
        
        public ReadyChecker readyChecker => _readyChecker;
        public GameOverWindow gameOverWindow => _gameOverWindow;
    }
}