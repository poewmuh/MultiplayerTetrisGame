using System;
using Cysharp.Threading.Tasks;
using Tetris.Gameplay.Core;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Tetris.Gameplay.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerHUD;
        
        public string userId => _userId.Value.ToString();
        public string nickname => _nickname.Value.ToString();
        
        private readonly NetworkVariable<FixedString32Bytes> _userId = new();
        private readonly NetworkVariable<FixedString32Bytes> _nickname = new();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                _userId.Value = Guid.NewGuid().ToString()[..6];
            }

            Register().Forget();
            InstantiateLocals().Forget();
        }

        public override void OnDestroy()
        {
            UnRegister();
            base.OnDestroy();
        }

        private async UniTaskVoid InstantiateLocals()
        {
            await UniTask.WaitWhile(() => !Session.Instance, cancellationToken: this.GetCancellationTokenOnDestroy());
            if ((IsClient || ServerIsHost) && IsOwner)
            {
                Instantiate(_playerHUD, null);
            }
        }

        private async UniTaskVoid Register()
        {
            await UniTask.WaitWhile(() => !PlayerControllerManager.Instance, cancellationToken: this.GetCancellationTokenOnDestroy());
            PlayerControllerManager.Instance.Register(this);
        }

        private void UnRegister()
        {
            if (PlayerControllerManager.Instance)
            {
                PlayerControllerManager.Instance.UnRegister(this);
            }
        }
    }
}