using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    public class Credits: AMenuState
    {
        public Credits() : base("Menus/Credits") { }

        public override void Enter()
        {
            base.Enter();
            
            Time.timeScale = 1f;
        }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Back:
                    OnEscPressed();
                    break;
                default:
                    Debug.LogWarning($"Credits::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    break;
            }
        }

        protected override void OnEscPressed()
        {
            MenuManager.Instance.ChangeSceneAndState("MainMenuScene", new Main());
        }
        
        public override void Update(float deltaTime) { }
    }
}