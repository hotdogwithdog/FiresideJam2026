using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus
{
    public class NavigationEvent: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private NavigationActions _action;
        public Action<NavigationActions> OnClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(_action);
        }
    }
}