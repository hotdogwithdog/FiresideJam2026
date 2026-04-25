using LevelControl;
using UI.Menus.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus.States
{
    public class EndLevel : AMenuState
    {
        public EndLevel() : base("Menus/LevelEnd") { }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.NextLevel:
                    GoToNextLevel();
                    break;
                case NavigationActions.MainMenu:
                    MenuManager.Instance.SetState(new Main());
                    break;
                default:
                    Debug.LogWarning($"EndLevel::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    return;
            }
        }

        private void GoToNextLevel()
        {
            LevelManager.Instance.CurrentLevel++;
            string nextSceneName = LevelManager.Instance.GetLevelName(LevelManager.Instance.CurrentLevel);
            MenuManager.Instance.ChangeSceneAndState(nextSceneName, new Gameplay());
        }
        
        protected override void OnEscPressed()
        {
            MenuManager.Instance.SetState(new Main());
        }

        public override void Update(float deltaTime) { }
    }
}