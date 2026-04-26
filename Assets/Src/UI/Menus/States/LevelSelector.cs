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
                    break;
                case NavigationActions.Level2:
                    LevelManager.Instance.CurrentLevel = 2;
                    break;
                case NavigationActions.Level3:
                    LevelManager.Instance.CurrentLevel = 3;
                    break;
                case NavigationActions.Level4:
                    LevelManager.Instance.CurrentLevel = 4;
                    break;
                case NavigationActions.Level5:
                    LevelManager.Instance.CurrentLevel = 5;
                    break;
                case NavigationActions.Level6:
                    LevelManager.Instance.CurrentLevel = 6;
                    break;
                case NavigationActions.Back:
                    MenuManager.Instance.SetState(new Main());
                    return;
                default:
                    Debug.LogWarning($"LevelSelector::OnOptionsClicked: The Navigation action is undefined in this menu -> navigation action {action}");
                    return;
            }
            
            MenuManager.Instance.ChangeSceneAndState(LevelManager.Instance.GetCurrentLevelName(), new Gameplay());
        }
        
        protected override void OnEscPressed()
        {
            MenuManager.Instance.SetState(new Main());
        }

        public override void Update(float deltaTime) { }
    }
}