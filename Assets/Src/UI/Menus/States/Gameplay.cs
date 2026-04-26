using Inputs;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Menus.States
{
    public class Gameplay : IState
    {
        public void Enter()
        {
            InputReader.Instance.onPause += OnPause;
            
            InputReader.Instance.EnablePlayerActions();
            
            Time.timeScale = 1f;
        }

        private void OnPause()
        {
            MenuManager.Instance.SetState(new Pause());
        }

        public void Exit()
        {
            InputReader.Instance.onPause -= OnPause;
            InputReader.Instance.EnableJustCommonActions();
        }

        public void Update(float deltaTime) { }
    }
}