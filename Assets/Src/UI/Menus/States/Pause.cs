using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    public class Pause: AMenuState
    {
        public Pause() : base("Menus/Pause") { }

        public override void Enter()
        {
            base.Enter();
            
            Time.timeScale = 0f;
        }
        
        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Resume:
                    // TODO: If the gameplay state have something that wants to store it needs to be passed in this constructor by the gameplay state itself for restore that same instance of gameplay
                    MenuManager.Instance.SetState(new Gameplay());
                    break;
                case NavigationActions.Options:
                    MenuManager.Instance.SetState(new Options());
                    break;
                case NavigationActions.MainMenu:
                    MenuManager.Instance.SetState(new Main());
                    break;
                default:
                    Debug.LogWarning($"Pause::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    break;
            }
        }
        
        protected override void OnEscPressed()
        {
            MenuManager.Instance.SetState(new Gameplay());
        }

        public override void Update(float deltaTime) { }
    }
}