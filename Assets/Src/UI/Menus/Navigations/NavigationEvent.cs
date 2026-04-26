using System;
using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus.Navigation
{
    public class NavigationEvent: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private NavigationActions _action;
        public Action<NavigationActions> onNavigationClick;
        [Header("Audio")]
        [SerializeField] private AudioClip _activateAudioClip;
        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot2D(_activateAudioClip,1f);
            onNavigationClick?.Invoke(_action);
        }
    }
}