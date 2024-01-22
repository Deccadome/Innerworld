using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Events;
using System.Diagnostics;

namespace Dome { 
    [CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
    public class InputReader : ScriptableObject, GameInput.IIWActions, GameInput.IOWActions, GameInput.IUIActions
    {
        public GameInput _gameInput;

        private void OnEnable()
        {
            if(_gameInput == null)
            {
                _gameInput = new GameInput();

                _gameInput.IW.SetCallbacks(this);
                _gameInput.OW.SetCallbacks(this);
                _gameInput.UI.SetCallbacks(this);

                if (GameObject.Find("OW SceneLoader") != null) SetOW();
                else if (GameObject.Find("IW SceneLoader") != null) SetIW();
                else SetUI();
            }
        }

        public void SetIW()
        {

            UnityEngine.Debug.Log(new StackTrace());
            _gameInput.IW.Enable();
            _gameInput.UI.Disable();
            _gameInput.OW.Disable();
        }

        public void SetOW()
        {
            _gameInput.OW.Enable();
            _gameInput.IW.Disable();
            _gameInput.UI.Disable();
        }

        public void SetUI()
        {
            _gameInput.UI.Enable();
            _gameInput.IW.Disable();
            _gameInput.OW.Disable();
        }

        public event Action MoveLeftEvent;
        public event Action MoveLeftCancelledEvent;
        public event Action MoveRightEvent;
        public event Action MoveRightCancelledEvent;
        public event Action MoveUpEvent;
        public event Action MoveUpCancelledEvent;
        public event Action MoveDownEvent;
        public event Action MoveDownCancelledEvent;

        public event Action SwitchWorldEvent;
        public event Action InteractEvent;
        public event Action AttackEvent;
        public event Action ShootEvent;
        public event Action<Vector2> AimEvent;

        public event Action PauseEvent;
        public event Action UnpauseEvent;

        public event Action<Vector2> PointEvent;
        public event Action ClickEvent;

        public void OnMoveLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) { MoveLeftEvent?.Invoke(); }
            if (context.phase == InputActionPhase.Canceled) { MoveLeftCancelledEvent?.Invoke(); }
        }
        public void OnMoveRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) MoveRightEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled) MoveRightCancelledEvent?.Invoke();
        }
        public void OnMoveUp(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) MoveUpEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled) MoveUpCancelledEvent?.Invoke();
        }
        public void OnMoveDown(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) MoveDownEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled) MoveDownCancelledEvent?.Invoke();
        }

        public void OnSwitchWorld(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed) SwitchWorldEvent?.Invoke();
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) InteractEvent?.Invoke();
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) AttackEvent?.Invoke();
        }
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) ShootEvent?.Invoke();
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            AimEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed) PauseEvent?.Invoke();
        }


        // IUIActions
        public void OnNavigate(InputAction.CallbackContext context)
        {
        
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            PointEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) ClickEvent?.Invoke();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {

        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {

        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) UnpauseEvent?.Invoke();
        }
    }
}
