using System;

namespace Tetris.Gameplay.Core
{
    public interface IGameModeSystem
    {
        event Action OnTeamWin;
        event Action OnGameOver;
    }
}