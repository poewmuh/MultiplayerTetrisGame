using System;
using Tetris.Gameplay.Tetris;
using UnityEngine;

namespace Tetris.Gameplay.Core
{
    public interface IGameModeSystem
    {
        event Action OnGameOver;

        void SpawnPiece(ulong ownerId);
        float GetFallDelay();
    }
}