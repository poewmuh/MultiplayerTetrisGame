using System;
using System.Collections.Generic;
using Tetris.Tools;

namespace Tetris.Gameplay.Player
{
    public class PlayerControllerManager : MonoSingleton<PlayerControllerManager>
    {
        public event Action<PlayerController> OnPlayerControllerReceived;
        
        private readonly List<PlayerController> _playerControllers = new();

        public void Register(PlayerController playerController)
        {
            _playerControllers.Add(playerController);
            OnPlayerControllerReceived?.Invoke(playerController);
        }

        public void UnRegister(PlayerController playerController)
        {
            _playerControllers.Remove(playerController);
        }
        
        public List<PlayerController> GetPlayersController() => _playerControllers;
    }
}