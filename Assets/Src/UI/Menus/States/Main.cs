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
                    MenuManager.Instance.SetState(new Play());
                    break;
                case NavigationActions.Options:
                    MenuManager.Instance.SetState(new Options(true));
                    break;
                case NavigationActions.Credits:
                    MenuManager.Instance.SetState(new Credits());
                    break;
            }
        }

        public override void Update(float deltaTime)
        {
        }
    }
}