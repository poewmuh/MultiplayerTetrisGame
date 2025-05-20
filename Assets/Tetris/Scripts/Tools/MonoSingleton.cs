using UnityEngine;

namespace Tetris.Tools
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => _instance as T;
        private static MonoSingleton<T> _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Debug.LogError($"{typeof(T).Name} singleton duplication in {gameObject.name}");
                Destroy(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

    }
}