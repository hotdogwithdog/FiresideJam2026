using System;
using Player;
using UnityEngine;

namespace UI.GameplayUI
{
    [RequireComponent(typeof(RectTransform))]
    public class MassBar : MonoBehaviour
    {
        private PlayerController _playerController;

        private RectTransform _rectTransform;
        private float _maxWidth;
        private float _maxMass;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _maxWidth = _rectTransform.rect.width;
            
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < objs.Length; ++i)
            {
                _playerController = objs[i].GetComponent<PlayerController>();
                if (_playerController != null) break;
            }
            
            _playerController.onMassChanged += OnMassChanged;
        }

        private void OnMassChanged(float newMass)
        {
            float newWidth = (newMass / _playerController.MaxMass) * _maxWidth;
            _rectTransform.sizeDelta = new Vector2(newWidth, _rectTransform.sizeDelta.y);
        }
    }
}

