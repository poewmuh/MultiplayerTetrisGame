using System;
using UnityEngine;

namespace Tetris.Tools
{
    public class Timer
    {
        public float CurrentTime;
        public float TotalTime;
        public bool Pause = true;
        public bool IsOver;

        public Action OnTimeIsOver;
        
        public Timer()
        {
            if(TimeBehaviour.Instance == null)
            {
                Debug.LogError($"[Timer] TimeBehaviour not exist");
                return;
            }
            else
            {
                TimeBehaviour.Instance.RegisterTimer(this);
            }
        }
        
        public Timer Run()
        {
            Pause = false;
            return this;
        }
        
        public Timer Stop()
        {
            Pause = true;
            OnTimeIsOver = null;
            IsOver = true;
            TimeBehaviour.Instance.UnregisterTimer(this);
            return this;
        }
        
        public void Tick(float deltaTime)
        {
            if (Pause) return;
            
            CurrentTime += deltaTime;
            if (CurrentTime >= TotalTime)
            {
                CurrentTime = TotalTime;
                OnTimeIsOver?.Invoke();
                Stop();
            }
        }
    }
}