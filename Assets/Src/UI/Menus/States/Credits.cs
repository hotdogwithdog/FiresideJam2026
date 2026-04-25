using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    public class Credits: AMenuState
    {
        public Credits() : base("Menus/Credits") { }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Back:
                    MenuManager.Instance.SetState(MenuManager.Instance.PreviousState);
                    break;
                default:
                    Debug.LogWarning($"Credits::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    break;
            }
        }

        public override void Update(float deltaTime) { }
    }
}