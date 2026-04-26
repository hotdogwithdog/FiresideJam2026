using System;
using Inputs.Generated;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputReader : Utilities.Singleton<InputReader>, Actions.IPlayerActions, Actions.ICommonActions
    {
        public Actions actions;
        
        #region PlayerEvents

        public Action<Vector2> onMove = delegate { };
        public Action onJump = delegate { };

        public Action<Vector2> onShootMass = delegate { };
        public Action onRespawnRequest = delegate { };

        #endregion
        
        #region CommonEvents

        public Action onPause = delegate { };
        
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
            actions.Common.SetCallbacks(this);
            //actions.UI.SetCallbacks(this);
            
            actions.Enable();
            EnableJustCommonActions();
        }

        private void OnDisable()
        {
            if (actions == null) return;
            actions.Player.SetCallbacks(null);
            actions.Common.SetCallbacks(null);
            actions.UI.SetCallbacks(null);
            actions.Disable();
        }

        public void EnableJustCommonActions()
        {
            actions.Common.Enable();
            actions.Player.Disable();
            actions.UI.Disable();
        }
        
        public void EnablePlayerActions()
        {
            actions.Common.Enable();
            actions.Player.Enable();
            actions.UI.Disable();
        }

        public void EnableUIActions()
        {
            actions.Common.Enable();
            actions.UI.Enable();
            actions.Player.Disable();
        }

        public void DisableAllActions()
        {
            actions.Common.Disable();
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

        #region CommonsActions
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) onPause.Invoke();
        }
        #endregion
        
    }
}