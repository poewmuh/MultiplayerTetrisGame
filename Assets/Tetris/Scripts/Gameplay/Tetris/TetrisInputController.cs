using System;
using Tetris.Data;
using Tetris.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.Gameplay.Tetris
{
    public class TetrisInputController : MonoSingleton<TetrisInputController>
    {
        public event Action OnMoveLeft;
        public event Action OnMoveRight;
        public event Action OnMoveDown;
        public event Action OnRotate;
        
        private TetrisControls _controls;

        private bool _holdLeft;
        private bool _holdRight;
        private bool _holdDown;
        private bool _holdRotate;
        private float _startDelay = 0.2f;
        private float _repeatDelay = 0.1f;
        private float _timerLeft;
        private float _timerRight;
        private float _timerDown;
        private float _timerRotate;
        
        private Action<InputAction.CallbackContext> _moveLeftStarted;
        private Action<InputAction.CallbackContext> _moveLeftCanceled;
        private Action<InputAction.CallbackContext> _moveRightStarted;
        private Action<InputAction.CallbackContext> _moveRightCanceled;
        private Action<InputAction.CallbackContext> _moveDownStarted;
        private Action<InputAction.CallbackContext> _moveDownCanceled;
        private Action<InputAction.CallbackContext> _rotateStarted;
        private Action<InputAction.CallbackContext> _rotateCanceled;

        private Action<InputAction.CallbackContext> _moveLeftPerformed;
        private Action<InputAction.CallbackContext> _moveRightPerformed;
        private Action<InputAction.CallbackContext> _moveDownPerformed;
        private Action<InputAction.CallbackContext> _rotatePerformed;

        protected override void Awake()
        {
            base.Awake();
            _controls = new TetrisControls();
            
            _moveLeftStarted = ctx => StartHold(ref _holdLeft, ref _timerLeft);
            _moveLeftCanceled = ctx => StopHold(ref _holdLeft, ref _timerLeft);
            _moveRightStarted = ctx => StartHold(ref _holdRight, ref _timerRight);
            _moveRightCanceled = ctx => StopHold(ref _holdRight, ref _timerRight);
            _moveDownStarted = ctx => StartHold(ref _holdDown, ref _timerDown);
            _moveDownCanceled = ctx => StopHold(ref _holdDown, ref _timerDown);
            _rotateStarted = ctx => StartHold(ref _holdRotate, ref _timerRotate);
            _rotateCanceled = ctx => StopHold(ref _holdRotate, ref _timerRotate);

            _moveLeftPerformed = ctx => OnMoveLeft?.Invoke();
            _moveRightPerformed = ctx => OnMoveRight?.Invoke();
            _moveDownPerformed = ctx => OnMoveDown?.Invoke();
            _rotatePerformed = ctx => OnRotate?.Invoke();
        }

        private void OnEnable()
        {
            EnableControls();
        }

        private void OnDisable()
        {
            DisableControls();
        }

        public void EnableControls()
        {
            _controls.Enable();
            
            _controls.Gameplay.MoveLeft.started += _moveLeftStarted;
            _controls.Gameplay.MoveLeft.canceled += _moveLeftCanceled;
            _controls.Gameplay.MoveRight.started += _moveRightStarted;
            _controls.Gameplay.MoveRight.canceled += _moveRightCanceled;
            _controls.Gameplay.MoveDown.started += _moveDownStarted;
            _controls.Gameplay.MoveDown.canceled += _moveDownCanceled;
            _controls.Gameplay.Rotate.started += _rotateStarted;
            _controls.Gameplay.Rotate.canceled += _rotateCanceled;
            
            _controls.Gameplay.MoveLeft.performed += _moveLeftPerformed;
            _controls.Gameplay.MoveRight.performed += _moveRightPerformed;
            _controls.Gameplay.MoveDown.performed += _moveDownPerformed;
            _controls.Gameplay.Rotate.performed += _rotatePerformed;
        }
        
        public void DisableControls()
        {
            _holdLeft = false;
            _holdRight = false;
            _holdDown = false;
            _holdRotate = false;
            _timerRotate = 0;
            _timerDown = 0;
            _timerLeft = 0;
            _timerRight = 0;
            
            _controls.Gameplay.MoveLeft.started -= _moveLeftStarted;
            _controls.Gameplay.MoveLeft.canceled -= _moveLeftCanceled;
            _controls.Gameplay.MoveRight.started -= _moveRightStarted;
            _controls.Gameplay.MoveRight.canceled -= _moveRightCanceled;
            _controls.Gameplay.MoveDown.started -= _moveDownStarted;
            _controls.Gameplay.MoveDown.canceled -= _moveDownCanceled;
            _controls.Gameplay.Rotate.started -= _rotateStarted;
            _controls.Gameplay.Rotate.canceled -= _rotateCanceled;
            
            _controls.Gameplay.MoveLeft.performed -= _moveLeftPerformed;
            _controls.Gameplay.MoveRight.performed -= _moveRightPerformed;
            _controls.Gameplay.MoveDown.performed -= _moveDownPerformed;
            _controls.Gameplay.Rotate.performed -= _rotatePerformed;
            _controls.Disable();
        }
        
        private void StartHold(ref bool holdFlag, ref float timer)
        {
            holdFlag = true;
            timer = 0f;
        }

        private void StopHold(ref bool holdFlag, ref float timer)
        {
            holdFlag = false;
            timer = 0f;
        }
        
        private void Update()
        {
            HandleHold(ref _holdLeft, ref _timerLeft, OnMoveLeft);
            HandleHold(ref _holdRight, ref _timerRight, OnMoveRight);
            HandleHold(ref _holdDown, ref _timerDown, OnMoveDown);
            HandleHold(ref _holdRotate, ref _timerRotate, OnRotate);
        }
        
        private void HandleHold(ref bool holdFlag, ref float timer, System.Action action)
        {
            if (!holdFlag) return;

            timer += Time.deltaTime;
            if (timer >= _startDelay)
            {
                while (timer >= _startDelay)
                {
                    action?.Invoke();
                    timer -= _repeatDelay;
                }
            }
        }
    }
}