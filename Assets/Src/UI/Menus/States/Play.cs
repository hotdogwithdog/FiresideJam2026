namespace UI.Menus.States
{
    public class Play: AMenuState
    {
        public Play() : base("Menus/Play") { }

        protected override void OnOptionsClicked(NavigationActions action)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(float deltaTime) { }
    }
}