using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Tetris.Gameplay.Core;
using TMPro;
using UnityEngine;

namespace Tetris.Gameplay.UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countDownText;

        private int _countDownMax = 3;
        private int _countDownCurrent;

        private void Start()
        {
            Session.Instance.gameMode.OnStateChanged += OnGameStateChange;
        }

        private void OnGameStateChange(GameState state)
        {
            if (state == GameState.CountDown)
            {
                StartCountdown();
            }

            if (state == GameState.InGame)
            {
                Session.Instance.gameMode.OnStateChanged -= OnGameStateChange;
                Hide();
            }
        }

        private void StartCountdown()
        {
            _countDownCurrent = _countDownMax;
            _countDownText.text = _countDownCurrent.ToString();
            _countDownText.enabled = true;
            ChangeCountText().Forget();
        }

        private async UniTaskVoid ChangeCountText()
        {
            while (_countDownCurrent >= 1)
            {
                _countDownText.text = _countDownCurrent.ToString();
                _countDownCurrent--;
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            _countDownText.text = "GO~";
        }

        private void Hide()
        {
            _countDownText.enabled = false;
        }
    }
}