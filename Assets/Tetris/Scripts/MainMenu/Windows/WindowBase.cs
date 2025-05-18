using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tetris.MainMenu.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public event Action<WindowBase> onWindowCompleteClose;

        [SerializeField] protected WindowViewBase _view;
        [SerializeField] private bool _showOnEnable = true;
        
        public bool IsOpened { get; private set; }

        private void OnEnable()
        {
            if (_showOnEnable)
            {
                StartOpen();
            }
        }

        public virtual void StartOpen()
        {
            IsOpened = true;
            _view.Show();
        }

        public virtual void StartClose()
        {
            IsOpened = false;
            _view.Hide();
            WaitAndDestroy().Forget();
        }

        private async UniTaskVoid WaitAndDestroy()
        {
            await UniTask.WaitWhile(() => _view.IsShowed, cancellationToken:this.GetCancellationTokenOnDestroy());
            onWindowCompleteClose?.Invoke(this);
            Destroy(gameObject);
        }
    }
}