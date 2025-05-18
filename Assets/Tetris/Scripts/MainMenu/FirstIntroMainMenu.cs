using DG.Tweening;
using Tetris.Helpers;
using Tetris.MainMenu.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.MainMenu
{
    public class FirstIntroMainMenu : MonoBehaviour
    {
        private static bool isFirstIntro = true;

        private void Start()
        {
            var buttonsAlpha = isFirstIntro ? 0 : 1;
            gameObject.SetActive(isFirstIntro);
            isFirstIntro = false;
        }

        private void Update()
        {
            if (InputHelper.WasAnyButtonDown())
            {
                ShowButtonsAndDisable();
            }
        }

        private void ShowButtonsAndDisable()
        {
            gameObject.SetActive(false);
            WindowsManager.CreateWindow<MainMenuWindow>(MainMenuWindow.ASSET_PATH, transform.parent);
        }
    }
}