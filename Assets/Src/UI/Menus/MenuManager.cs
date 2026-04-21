using System;
using UI.Menus.States;
using UnityEngine;

namespace UI.Menus
{
    public class MenuManager: Utilities.Singleton<MenuManager>
    {
        private IState _currentState;
        private void Start()
        {
            SetState(new Main());
        }
        
        public void SetState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public IState GetState()
        {
            return _currentState;
        }

        public void Update()
        {
            _currentState?.Update(Time.deltaTime);
        }
    }
}