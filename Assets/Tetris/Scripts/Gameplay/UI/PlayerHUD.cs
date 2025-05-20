using Tetris.Tools;
using UnityEngine;

namespace Tetris.Gameplay.UI
{
    public class PlayerHUD : SharedBehaviour
    {
        [SerializeField] private ReadyChecker _readyChecker;
        //[SerializeField] private PingChecker _pingChecker;
        //[SerializeField] private FPSChecker _fpsChecker;
        
        public ReadyChecker readyChecker => _readyChecker;
    }
}