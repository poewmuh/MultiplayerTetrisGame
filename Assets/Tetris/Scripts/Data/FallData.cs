using System;
using UnityEngine;

namespace Tetris.Data
{
    [Serializable]
    public class FallData
    {
        [SerializeField] private float _defaltFallTime = .7f;
        [SerializeField] private float _minimalFallTime = .3f;
        [SerializeField] private float _fallTimeStep = .05f;
        [SerializeField] private float _timeForDecreaseFall = 15f;
        
        public float GetDefaltFallTime() => _defaltFallTime;
        public float GetMinimalFallTime() => _minimalFallTime;
        public float GetFallTimeStep() => _fallTimeStep;
        public float GetTimeForDecreaseFall() => _timeForDecreaseFall;
    }
}