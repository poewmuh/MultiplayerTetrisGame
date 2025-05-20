using System;

namespace Tetris.Tools
{
    public class ComponentResolver<T> where T : UnityEngine.Component
    {
        private T component;

        public T Component
        {
            get
            {
                if (component.IsNull())
                {
                    TryResolve();
                }

                return component;
            }
        }

        public T RawComponent => component;

        public event Action OnComponentRegistered;

        public ComponentResolver()
        {
            ComponentLocator.OnComponentRegistered += ComponentRegisteredHandler;
            ComponentLocator.OnComponentDestroyed += ComponentDestroyedHandler;

            TryResolve();
        }

        ~ComponentResolver()
        {
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            component = null;
            OnComponentRegistered = null;
            ComponentLocator.OnComponentRegistered -= ComponentRegisteredHandler;
            ComponentLocator.OnComponentDestroyed -= ComponentDestroyedHandler;
        }

        private void ComponentRegisteredHandler(Type type)
        {
            if (type == typeof(T))
            {
                TryResolve();
                OnComponentRegistered?.Invoke();
            }
        }

        private void ComponentDestroyedHandler(Type type)
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private bool TryResolve() => ComponentLocator.TryResolve<T>(out component);
    }
}