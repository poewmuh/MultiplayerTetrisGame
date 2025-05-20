using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Core
{
	public abstract class GameplaySystem<T> : NetworkBehaviour where T : NetworkBehaviour
	{
		public static T Instance => _instance as T;

		private static GameplaySystem<T> _instance;

		protected void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError($"[Gameplay] System: detect multiple instance {GetType()}");
			}

			_instance = this;
		}

		protected void Start()
		{
			Initialize();
		}

		protected abstract void Initialize();

		protected virtual void OnDestroy()
		{
			Debug.Log($"[Gameplay] System: destroyed instance {GetType()}");
			
			if (_instance == this)
			{
				_instance = null;
			}
		}
	}
}