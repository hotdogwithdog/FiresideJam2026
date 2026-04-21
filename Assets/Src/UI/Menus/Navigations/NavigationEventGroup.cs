using System;
using UnityEngine;

namespace UI.Menus
{
    public class NavigationEventGroup: MonoBehaviour
    {
        public Action<NavigationActions> OnClick;
        private NavigationEvent[] _navigationEvents;

        private void Start()
        {
            _navigationEvents = GetComponentsInChildren<NavigationEvent>(true);

            foreach (var navigationEvent in _navigationEvents)
            {
                navigationEvent.OnClick += OnNavigationClick;
            }
        }

        private void OnDestroy()
        {
            foreach (var navigationEvent in _navigationEvents)
            {
                navigationEvent.OnClick -= OnNavigationClick;
            }
        }


        private void OnNavigationClick(NavigationActions navigationAction)
        {
            OnClick?.Invoke(navigationAction);
        }
    }
}