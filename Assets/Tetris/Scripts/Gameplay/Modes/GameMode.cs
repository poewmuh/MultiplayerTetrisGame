namespace Tetris.Gameplay.Core
{
    public abstract class GameMode
    {
        public virtual float remainTime { get; private set; }
        public virtual GameState state { get; private set; }
        
        public virtual void StartGame() { }
        public virtual void Pause() { }
        public virtual void Update(float deltaTime)	{ }

        public virtual void FinishGame()
        {
            Session.Instance.FinishGame();
        }
    }
}