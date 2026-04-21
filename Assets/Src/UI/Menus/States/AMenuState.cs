using UnityEngine;

namespace UI.Menus.States
{
    public abstract class AMenuState: IState
    {
        protected string _menuName;
        protected GameObject _menu;
        protected Canvas _canvas;
        public AMenuState(string menuName)
        {
            _menuName = menuName;
        }
        public virtual void Enter()
        {
            _canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
            GameObject menuPrefab = Resources.Load(_menuName) as GameObject;
            _menu = GameObject.Instantiate(menuPrefab, _canvas.transform);
        }

        protected abstract void OnOptionsClicked(NavigationActions action);
        public virtual void Exit()
        {
            // if (_menu != null)
            // {
            //     _menu.GetComponentInChildren<MenuOptionsGroup>().onOptionClicked -= OnOptionClicked;
            //     GameObject.Destroy(_menu);
            // }
        }

        public abstract void Update(float deltaTime);
    }
}