using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus.Navigation
{
    public class NavigationEvent: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private NavigationActions _action;
        public Action<NavigationActions> onNavigationClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onNavigationClick?.Invoke(_action);
        }
    }
}