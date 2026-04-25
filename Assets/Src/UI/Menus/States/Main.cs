using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    public class Main: AMenuState
    {
        public Main() : base("Menus/MainMenu"){ }
        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Play:
                    // TODO: Change to the Level Selector
                    MenuManager.Instance.SetState(new LevelSelector());
                    break;
                case NavigationActions.Options:
                    MenuManager.Instance.SetState(new Options());
                    break;
                case NavigationActions.Credits:
                    MenuManager.Instance.SetState(new Credits());
                    break;
                case NavigationActions.Exit:
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #else
                    Application.Quit();
                    #endif
                    break;
                default:
                    Debug.LogWarning($"Main::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    return;
            }
        }

        public override void Update(float deltaTime)
        {
        }
    }
}