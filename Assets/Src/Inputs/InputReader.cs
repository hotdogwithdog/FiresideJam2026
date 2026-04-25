using System;
using Inputs.Generated;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputReader : Utilities.Singleton<InputReader>, Actions.IPlayerActions
    {
        public Actions actions;
        
        #region PlayerEvents

        public Action<Vector2> onMove = delegate { };
        public Action onJump = delegate { };

        public Action<Vector2> onShootMass = delegate { };
        public Action onRespawnRequest = delegate { };

        #endregion
        
        #region ActivatesAndDisables
        private void Awake()
        {
            base.Awake();
            actions = new Actions();
            actions.Disable();
        }

        private void OnEnable()
        {
            if (actions == null)
            {
                Debug.LogError("No actions found");
                return;
            }
            
            actions.Player.SetCallbacks(this);
            //actions.UI.SetCallbacks(this);
            
            actions.Enable();
        }

        private void OnDisable()
        {
            actions.Player.SetCallbacks(null);
            actions.UI.SetCallbacks(null);
            actions.Disable();
        }


        public void EnablePlayerActions()
        {
            actions.Player.Enable();
            actions.UI.Disable();
        }

        public void EnableUIActions()
        {
            actions.UI.Enable();
            actions.Player.Disable();
        }

        public void DisableAllActions()
        {
            actions.Player.Disable();
            actions.UI.Disable();
        }
        #endregion
        
        #region PlayerMapCallbacks
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled) 
                onMove.Invoke(context.ReadValue<Vector2>());
        }

        public void OnShootMass(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                onShootMass.Invoke(Mouse.current.position.ReadValue());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) onJump.Invoke();
        }

        public void OnRespawn(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) onRespawnRequest.Invoke();
        }

        #endregion
    }
}