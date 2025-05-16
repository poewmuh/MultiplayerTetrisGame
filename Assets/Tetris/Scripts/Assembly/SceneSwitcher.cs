using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tetris.Assembly
{
    public static class SceneSwitcher
    {
        public static SceneType currentScene { get; private set; }
        
        private static readonly Dictionary<SceneType, string> scenesPath = new ()
        {
            { SceneType.MainMenu, "MainMenu" },
            { SceneType.Gameplay, "Gameplay" }
        };
        
        public static void LoadScene(SceneType scene)
        {
            currentScene = scene;
            Addressables.LoadSceneAsync(scenesPath[scene]);
        }
    }
}