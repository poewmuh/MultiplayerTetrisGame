using DG.Tweening;
using UnityEngine;

namespace Tetris.MainMenu.Windows
{
    public abstract class WindowViewBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public bool IsShowed { get; private set; }

        public virtual void Show()
        {
            _canvasGroup.DOFade(1, WindowsConstants.WindowFadeTime).OnComplete(() => IsShowed = true);
        }

        public virtual void Hide()
        {
            _canvasGroup.DOFade(0, WindowsConstants.WindowFadeTime).OnComplete(() => IsShowed = false);
        }
    }
}