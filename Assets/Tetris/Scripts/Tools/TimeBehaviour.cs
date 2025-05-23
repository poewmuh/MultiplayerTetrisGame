using UnityEngine;
using System.Collections.Generic;

namespace Tetris.Tools
{
    public class TimeBehaviour : MonoSingleton<TimeBehaviour>
    {
        private readonly List<Timer> _timers = new();
        private readonly List<Timer> _unregisteredTimers = new();
        private float _deltaTime;

        public void RegisterTimer(Timer timer)
        {
            _timers.TryAdd(timer);
        }

        public void UnregisterTimer(Timer timer)
        {
            _unregisteredTimers.TryAdd(timer);
        }

        public void Update()
        {
            _deltaTime = Time.deltaTime;
            foreach(var t in _timers)
            {
                t.Tick(_deltaTime);
            }
            
            foreach (var timer in _unregisteredTimers)
            {
                _timers.Remove(timer);
            }
            
            _unregisteredTimers.Clear();
        }
    }
}