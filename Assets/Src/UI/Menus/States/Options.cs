using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    public class Options: AMenuState
    {
        public Options() : base("Menus/Options")
        {
        }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Back :
                    MenuManager.Instance.SetState(MenuManager.Instance.PreviousState);
                    break;
                default:
                    Debug.LogWarning($"Options::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    break;
            }
        }
        
        protected override void OnEscPressed()
        {
            MenuManager.Instance.SetState(MenuManager.Instance.PreviousState);
        }

        public override void Update(float deltaTime) { }
    }
}