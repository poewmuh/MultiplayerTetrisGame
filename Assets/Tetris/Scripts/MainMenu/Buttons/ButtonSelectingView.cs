using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tetris.MainMenu.Buttons
{
    public class ButtonSelectingView : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Image _hoverImage;
        [SerializeField] private bool _isSelectingOnEnable = false;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetSelected();
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            _hoverImage.enabled = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _hoverImage.enabled = false;
        }

        private void OnEnable()
        {
            if (_isSelectingOnEnable)
            {
                SetSelected();
            }
        }

        private void SetSelected()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}