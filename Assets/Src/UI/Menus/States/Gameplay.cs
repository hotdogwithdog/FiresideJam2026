using Inputs;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Menus.States
{
    public class Gameplay : IState
    {
        public void Enter()
        {
            // TODO: Have this input
            //InputReader.Instance.onPause += OnPause;
        }

        private void OnPause()
        {
            MenuManager.Instance.SetState(new Pause());
        }

        public void Exit()
        {
            //InputReader.Instance.onPause += OnPause;
        }

        public void Update(float deltaTime) { }
    }
}