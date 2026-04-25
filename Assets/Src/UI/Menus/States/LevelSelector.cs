using LevelControl;
using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    public class LevelSelector : AMenuState
    {
        public LevelSelector() : base("Menus/LevelSelector") { }

        // This shit code is just to do it fast for the jam
        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Level1:
                    LevelManager.Instance.CurrentLevel = 1;
                    MenuManager.Instance.ChangeSceneAndState("Level1", new Gameplay());
                    break;
                case NavigationActions.Level2:
                    LevelManager.Instance.CurrentLevel = 2;
                    MenuManager.Instance.ChangeSceneAndState("Level2", new Gameplay());
                    break;
                case NavigationActions.Level3:
                    LevelManager.Instance.CurrentLevel = 3;
                    MenuManager.Instance.ChangeSceneAndState("Level3", new Gameplay());
                    break;
                case NavigationActions.Level4:
                    LevelManager.Instance.CurrentLevel = 4;
                    MenuManager.Instance.ChangeSceneAndState("Level4", new Gameplay());
                    break;
                case NavigationActions.Level5:
                    LevelManager.Instance.CurrentLevel = 5;
                    MenuManager.Instance.ChangeSceneAndState("Level5", new Gameplay());
                    break;
                case NavigationActions.Level6:
                    LevelManager.Instance.CurrentLevel = 6;
                    MenuManager.Instance.ChangeSceneAndState("Level6", new Gameplay());
                    break;
                case NavigationActions.Back:
                    MenuManager.Instance.SetState(new Main());
                    break;
                default:
                    Debug.LogWarning($"LevelSelector::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    return;
            }
        }

        public override void Update(float deltaTime) { }
    }
}