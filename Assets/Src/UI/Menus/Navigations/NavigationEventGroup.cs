using System;
using UnityEngine;


namespace UI.Menus.Navigation
{
    public class NavigationEventGroup: MonoBehaviour
    {
        public Action<NavigationActions> onNavigationEvent;
        private NavigationEvent[] _navigationEvents;

        private void Start()
        {
            _navigationEvents = GetComponentsInChildren<NavigationEvent>(true);

            foreach (var navigationEvent in _navigationEvents)
            {
                navigationEvent.onNavigationClick += OnNavigationClick;
            }
        }

        private void OnDestroy()
        {
            foreach (var navigationEvent in _navigationEvents)
            {
                navigationEvent.onNavigationClick -= OnNavigationClick;
            }
        }


        private void OnNavigationClick(NavigationActions navigationAction)
        {
            onNavigationEvent?.Invoke(navigationAction);
        }
    }
}