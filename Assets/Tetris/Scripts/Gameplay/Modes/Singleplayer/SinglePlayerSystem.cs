using System;
using Tetris.Gameplay.Datas;

namespace Tetris.Gameplay.Core
{
    public class SinglePlayerSystem : GameplaySystem<SinglePlayerSystem>, IGameModeSystem
    {
        public event Action OnTeamWin;
        public event Action OnGameOver;
        
        public SinglePlayerData Data { get; private set; }

        protected override void Initialize()
        {
            
        }
    }
}