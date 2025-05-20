using UnityEngine;

namespace Tetris.Tools
{
    public class SharedBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            ComponentLocator.Register(this);
            Init();
        }

        protected virtual void OnDestroy()
        {
            ComponentLocator.OnDestroy(this);
            Release();
        }

        protected virtual void Init()
        {

        }

        protected virtual void Release()
        {

        }
    }
}