using System;
using Tetris.Gameplay.Datas;

namespace Tetris.Gameplay.Core
{
    public class TwoPlayersSystem : GameplaySystem<TwoPlayersSystem>, IGameModeSystem
    {
        public event Action OnTeamWin;
        public event Action OnGameOver;
        
        public TwoPlayersData Data { get; private set; }

        protected override void Initialize()
        {
            
        }
    }
}