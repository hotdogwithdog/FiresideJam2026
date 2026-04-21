using UnityEngine;

namespace UI.Menus.States
{
    public class Options: AMenuState
    {
        private bool _isMainMenu;

        public Options(bool isMainMenu) : base("Menus/Options")
        {
            _isMainMenu = isMainMenu;
        }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Back :
                    Back();
                    break;
                default:
                    Debug.LogError("Unknown action: " + action);
                    break;
            }
        }

        private void Back()
        {
            if (_isMainMenu)
            {
                MenuManager.Instance.SetState(new Main());
            }
            else
            {
                MenuManager.Instance.SetState(new Pause());
            }
        }

        public override void Update(float deltaTime) { }
    }
}