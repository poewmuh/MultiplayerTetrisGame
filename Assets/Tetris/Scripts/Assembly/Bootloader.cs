using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tetris.Assembly
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private ShaderVariantCollection _preloadShaders;

        private void Start()
        {
            //_preloadShaders.WarmUp();
            LoadMainMenu().Forget();
        }

        private async UniTaskVoid LoadMainMenu()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            SceneSwitcher.LoadScene(SceneType.MainMenu);
        }
    }
}