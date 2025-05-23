using System;
using Tetris.Data;

namespace Tetris.Gameplay.Tetris
{
    public class FallHandler
    {
        public Action<float> OnFallTimeChanged;
        
        private FallData _fallData;
        private float _currentTimer;
        private bool _isPaused = true;
        private float _currentFallTime;
        
        public FallHandler(FallData fallData)
        {
            _fallData = fallData;
            ResetFallTime();
        }

        public void SetPauseState(bool active)
        {
            _isPaused = active;
            _currentTimer = 0;
            OnFallTimeChanged?.Invoke(_currentFallTime);
        }

        public void ResetFallTime()
        {
            _currentFallTime = _fallData.GetDefaltFallTime();
            _currentTimer = 0;
            OnFallTimeChanged?.Invoke(_currentFallTime);
        }
        
        public void Tick(float deltaTime)
        {
            if (_isPaused || _currentFallTime <= _fallData.GetMinimalFallTime()) return;
            _currentTimer += deltaTime;
            if (_currentTimer > _fallData.GetTimeForDecreaseFall())
            {
                _currentTimer = 0;
                _currentFallTime -= _fallData.GetFallTimeStep();
                OnFallTimeChanged?.Invoke(_currentFallTime);
            }
        }
    }
}