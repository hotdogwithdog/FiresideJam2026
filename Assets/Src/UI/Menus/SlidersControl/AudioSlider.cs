using System;
using Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace UI.Menus.SlidersControl
{
    [RequireComponent(typeof(UnityEngine.UI.Slider))]
    public class AudioSlider : MonoBehaviour, IDragHandler, IDropHandler, IPointerDownHandler
    {
        [SerializeField] private AudioChannel _audioChannel;

        private UnityEngine.UI.Slider _slider;
        
        private void Start()
        {
            _slider = GetComponent<UnityEngine.UI.Slider>();
            
            _slider.value = AudioManager.Instance.GetVolumeByChannel(_audioChannel);
        }

        private void SetVolume()
        {
            AudioManager.Instance.SetVolume(_audioChannel, _slider.value);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetVolume();
        }

        public void OnDrop(PointerEventData eventData)
        {
            SetVolume();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetVolume();
        }
    }
}