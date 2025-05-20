using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay.UI
{
    public class ReadyChecker : MonoBehaviour
    {
        public event Action OnPlayerReady;
        
        [SerializeField] private Button _readyButton;
        [SerializeField] private TextMeshProUGUI _waitingForOthersText;
        [SerializeField] private GameObject _holder;

        private void Start()
        {
            _readyButton.onClick.AddListener(OnReadyButtonClick);
            Show();
        }

        private void OnReadyButtonClick()
        {
            _readyButton.gameObject.SetActive(false);
            _waitingForOthersText.enabled = true;
            OnPlayerReady?.Invoke();
        }

        public void Show()
        {
            _readyButton.gameObject.SetActive(true);
            _holder.gameObject.SetActive(true);
            _waitingForOthersText.enabled = false;
        }

        public void Hide()
        {
            _holder.gameObject.SetActive(false);
            _waitingForOthersText.enabled = false;
        }
    }
}