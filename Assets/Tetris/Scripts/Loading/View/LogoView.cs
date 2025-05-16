using System;
using DG.Tweening;
using UnityEngine;

namespace Tetris.Loading
{
    public class LogoView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _logoCanvasGroup;
        [SerializeField] private float _fadeTime = 1;
        
        private Sequence _fadeSequence;

        private void Awake()
        {
            _fadeSequence = DOTween.Sequence();
            _fadeSequence
                .Append(_logoCanvasGroup.DOFade(1, _fadeTime))
                .AppendInterval(1)
                .Append(_logoCanvasGroup.DOFade(0, _fadeTime));
        }
    }
}