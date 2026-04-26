using UnityEngine;

namespace UI.Menus.States
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Update(float deltaTime);
    }
}
