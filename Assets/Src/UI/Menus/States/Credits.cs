namespace UI.Menus.States
{
    public class Credits: AMenuState
    {
        public Credits() : base("/Menus/Credits") { }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            switch (action)
            {
                case NavigationActions.Back:
                    MenuManager.Instance.SetState(new Main());
                    break;
            }
        }

        public override void Update(float deltaTime) { }
    }
}