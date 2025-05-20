using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tetris.Tools
{
    public static class ComponentLocator
    {
        private static readonly Dictionary<Type, Component> sharedComponents = new ();
        private static event Action<Type> _onComponentRegistered;

        public static event Action<Type> OnComponentRegistered;
        public static event Action<Type> OnComponentDestroyed;

        public static bool TryResolve<T>(out T component) where T : Component
        {
            if (sharedComponents.TryGetValue(typeof(T), out var result))
            {
                component = (T)result;
                return true;
            }

            component = null;
            return false;
        }

        public static async UniTask<T> ResolveAsync<T>(float awaitTime = 1.0f) where T : Component
        {
            Type componentType = typeof(T);

            if (sharedComponents.ContainsKey(componentType))
            {
                return Resolve<T>(componentType);
            }

            var taskCompletionSource = new UniTaskCompletionSource<bool>();
            _onComponentRegistered += OnComponentRegistered;

            var delayTask = UniTask.Delay(TimeSpan.FromSeconds(awaitTime));
            var result = await UniTask.WhenAny(taskCompletionSource.Task, delayTask);

            if (!result.hasResultLeft)
            {
                Debug.LogError($"Component of type {componentType.Name} didn't find");
            }

            return Resolve<T>(componentType);

            void OnComponentRegistered(Type type)
            {
                _onComponentRegistered -= OnComponentRegistered;
                if (type == componentType) taskCompletionSource.TrySetResult(true);
            }
        }

        public static async UniTask<T> ResolveOrCreateAsync<T>(float awaitTime = 1.0f) where T : Component
        {
            Type componentType = typeof(T);

            if (sharedComponents.ContainsKey(componentType))
            {
                return Resolve<T>(componentType);
            }

            var taskCompletionSource = new UniTaskCompletionSource<bool>();
            _onComponentRegistered += OnComponentRegistered;

            var delayTask = UniTask.Delay(TimeSpan.FromSeconds(awaitTime));
            var result = await UniTask.WhenAny(taskCompletionSource.Task, delayTask);

            if (!result.hasResultLeft)
            {
                CreateInstance<T>(componentType);
            }

            return Resolve<T>(componentType);

            void OnComponentRegistered(Type type)
            {
                _onComponentRegistered -= OnComponentRegistered;
                if (type == componentType) taskCompletionSource.TrySetResult(true);
            }
        }

        public static T Resolve<T>() where T : Component
        {
            return Resolve<T>(typeof(T));
        }

        public static T ResolveOrCreate<T>() where T : Component
        {
            var type = typeof(T);
            T result = Resolve<T>(type);
            if (result == null)
            {
                result = CreateInstance<T>(type);
            }

            return result;
        }

        public static T ResolveOrFind<T>() where T : Component
        {
            var result = Resolve<T>(typeof(T));
            if (result.IsNotNull())
            {
                return result;
            }

            result = UnityEngine.Object.FindObjectOfType<T>();
            if (result.IsNull())
            {
                return null;
            }

            Register(result);
            return result;
        }

        public static void Register<T>(T component) where T : Component
        {
            RegisterSharedComponent(component, component.GetType());
        }

        public static void RegisterByType<T>(T component) where T : Component
        {
            RegisterSharedComponent(component, typeof(T));
        }

        public static void OnDestroy<T>(T component) where T : Component
        {
            var type = component.GetType();
            if (!sharedComponents.TryGetValue(type, out var sharedComponent))
            {
                return;
            }

            if (sharedComponent.GetInstanceID() == component.Ref()?.GetInstanceID())
            {
                RemoveSharedComponent(type);
            }
        }

        public static void DestroyByType<T>() where T : Component
        {
            var type = typeof(T);
            RemoveSharedComponent(type);
        }

        private static T Resolve<T>(Type type) where T : Component
        {
            if (sharedComponents.TryGetValue(type, out var result))
            {
                return (T)result;
            }

            return null;
        }

        private static void RegisterSharedComponent<T>(T component, Type type) where T : UnityEngine.Component
        {
            if (sharedComponents.TryGetValue(type, out var registeredComponent))
            {
                if (registeredComponent.IsNull())
                {
                    sharedComponents[type] = component;
                    return;
                }

                if (registeredComponent.GetInstanceID() == component.GetInstanceID())
                {
                    return;
                }

                Debug.LogError($"Several components were found {component.gameObject} will be destroyed");
                UnityEngine.Object.Destroy(component.gameObject);
                return;
            }

            sharedComponents.Add(type, component);
            _onComponentRegistered?.Invoke(type);
            OnComponentRegistered?.Invoke(type);
        }

        private static T CreateInstance<T>(Type type) where T : UnityEngine.Component
        {
            var gameObject = new GameObject($"[Shared] {type.Name}");
            var sharedComponent = gameObject.AddComponent<T>();
            RegisterSharedComponent<T>(sharedComponent, type);
            return sharedComponent;
        }

        private static void RemoveSharedComponent(Type type)
        {
            if (sharedComponents.Remove(type))
                OnComponentDestroyed?.Invoke(type);
        }

        public static void ResetStatic()
        {
            sharedComponents.Clear();
            _onComponentRegistered = default;
            OnComponentRegistered = default;
            OnComponentDestroyed = default;
        }
    }
}
