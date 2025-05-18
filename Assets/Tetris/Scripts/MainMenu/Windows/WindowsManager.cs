using System.Collections.Generic;
using System.Linq;
using Tetris.Tools;
using UnityEngine;

namespace Tetris.MainMenu.Windows
{
    public class WindowsManager
    {
        private static readonly Dictionary<WindowBase, AssetLoaderHandler> _openedWindowAssets = new();
        private static WindowBase _previousWindow;
        
        public static T CreateWindow<T>(string path, Transform parent = null) where T : WindowBase
        {
            if (_openedWindowAssets.Keys.Any(w => w.GetType() == typeof(T)))
            {
                Debug.Log("[WindowsManager] Window already open");
                return null;
            }

            if (_previousWindow != null)
            {
                _previousWindow.StartClose();
            }

            var loadHandler = new AssetLoaderHandler();
            var windowObject = loadHandler.LoadGOImmediate<T>(path);
            var window = GameObject.Instantiate(windowObject, parent);
            window.onWindowCompleteClose += OnWindowClose;
            _openedWindowAssets.Add(window, loadHandler);
            _previousWindow = window;
            return window;
        }
        
        private static void OnWindowClose(WindowBase window)
        {
            window.onWindowCompleteClose -= OnWindowClose;
            _openedWindowAssets[window].Unload();
            _openedWindowAssets.Remove(window);
        }
    }
}