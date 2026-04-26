using System;
using UI.Menus.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class MenuManager: Utilities.Singleton<MenuManager>
    {
        private IState _currentState;
        private IState _previousState;

        private IState _stateToChangeOnSceneLoaded;
        
        private void Start()
        {
            _previousState = null;
            SetState(new Main());
        }
        
        public IState CurrentState => _currentState;
        public IState PreviousState => _previousState;
        
        public void SetState(IState newState)
        {
            _currentState?.Exit();
            
            _previousState = _currentState;
            
            _currentState = newState;
            
            _currentState.Enter();
        }

        public void ChangeSceneAndState(string sceneName, IState stateToChangeWhenLoaded)
        {
            _stateToChangeOnSceneLoaded = stateToChangeWhenLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetState(_stateToChangeOnSceneLoaded);
            _stateToChangeOnSceneLoaded = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Update()
        {
            _currentState?.Update(Time.deltaTime);
        }
    }
}