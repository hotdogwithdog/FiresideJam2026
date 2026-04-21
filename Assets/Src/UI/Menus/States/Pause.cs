namespace UI.Menus.States
{
    public class Pause: AMenuState
    {
        public Pause() : base("Menus/Pause") { }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.MainMenu:
                    MenuManager.Instance.SetState(new Main());
                    break;
                case NavigationActions.Resume:
                    MenuManager.Instance.SetState(new Play());
                    break;
                case NavigationActions.Options:
                    MenuManager.Instance.SetState(new Options(false));
                    break;
            }
        }

        public override void Update(float deltaTime) { }
    }
}