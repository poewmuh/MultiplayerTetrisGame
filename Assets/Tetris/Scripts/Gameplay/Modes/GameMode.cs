using System;
using Tetris.Tools;
using UnityEngine;

namespace Tetris.Gameplay.Core
{
    public abstract class GameMode
    {
        public event Action<GameState> OnStateChanged;
        public GameState currentState { get; private set; }
        
        public virtual int playersCount => 2;
        public virtual float remainTime { get; protected set; }

        private Timer _countDownTimer;

        public virtual void StartGame()
        {
            SetState(GameState.CountDown);
            _countDownTimer = new Timer()
            {
                TotalTime = 4,
                OnTimeIsOver = () => SetState(GameState.InGame)
            };
            _countDownTimer.Run();

            Session.Instance.gameModeSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver(ulong looserTeam)
        {
            Session.Instance.gameModeSystem.OnGameOver -= OnGameOver;
            SetState(GameState.GameOver);
        }

        public virtual void SetState(GameState newState)
        {
            Debug.Log($"[GameMode] State changed : {newState}");
            currentState = newState;
            OnStateChanged?.Invoke(newState);
        }
        public virtual void Pause() { }
        public virtual void Update(float deltaTime)	{ }

        public virtual void FinishGame()
        {
            Session.Instance.FinishGame();
        }
    }
}