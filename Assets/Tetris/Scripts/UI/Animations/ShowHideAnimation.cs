using DG.Tweening;
using UnityEngine;

namespace Tetris.UI.Animations
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ShowHideAnimation : MonoBehaviour
    {
        private Sequence _sequence;
        private CanvasGroup _canvasGroup;
        [SerializeField] private float _timeToHide = 1;
        [SerializeField] private float _timeToShow = 1;
        [SerializeField] private float _delayTimeInHide = 0.1f;
        [SerializeField] private float _delayTimeInShow = 0.5f;
        [SerializeField] private float _hideAlpha = 0;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _canvasGroup.alpha = 1;
            _sequence = DOTween.Sequence();
            _sequence.Append(_canvasGroup.DOFade(_hideAlpha, _timeToHide))
                .AppendInterval(_delayTimeInHide)
                .Append(_canvasGroup.DOFade(1, _timeToShow))
                .AppendInterval(_delayTimeInShow)
                .SetLoops(-1);
        }

        private void OnDisable()
        {
            DOTween.Kill(_sequence);
            _sequence = null;
            _canvasGroup.alpha = 0;
        }


    }
}