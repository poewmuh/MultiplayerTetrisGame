using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tetris.Helpers
{
    public class UniTaskHelper
    {
        public static async UniTaskVoid WaitAFrameAndDO(int frames, Action action)
        {
            await UniTask.DelayFrame(frames);
            action?.Invoke();
        }
        
        public static async UniTaskVoid WaitAndDo(float delay, Action action)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            action?.Invoke();
        }
        
        public static async UniTaskVoid WaitAndDo(GameObject go, float delay, Action action)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: go.GetCancellationTokenOnDestroy());
            action?.Invoke();
        }
    }
}