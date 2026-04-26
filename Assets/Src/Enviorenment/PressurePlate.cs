using System;
using MassInteraction;
using UnityEngine;

namespace Environment
{
    public class PressurePlate : ASoftBodyInteract, IReseteable
    {
        [SerializeField] private ButtonMode _buttonMode = ButtonMode.PressurePlate;
        [SerializeField] private GameObject[] _objectsToActivate;
        private IActivable[] _activables;
        
        [Header("Sprites")]
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _DeactiveSprite;
        
        
        [SerializeField] private bool _isActive;
        private bool _isActiveStartState;
        private int _numberOfSoftBodiesOnPlate = 0;
        
        private enum ButtonMode
        {
            PressurePlate,
            Button
        }

        private SpriteRenderer _spriteRenderer;
        private void Start()
        {
            _isActiveStartState = _isActive;
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _isActive? _activeSprite : _DeactiveSprite;
            _activables = new IActivable[_objectsToActivate.Length];
            for (int i = 0; i < _objectsToActivate.Length; i++)
            {
                _activables[i] = _objectsToActivate[i].GetComponent<IActivable>();
            }
        }
        
        protected override void OnSoftBodyEntered(IMass softBodyMass)
        {
            _numberOfSoftBodiesOnPlate++;
            ActivatePressurePlate();
        }

        protected override void OnSoftBodyStay(IMass softBodyMass)
        {
            
        }

        protected override void OnSoftBodyExit(IMass softBodyMass)
        {
            _numberOfSoftBodiesOnPlate--;
            switch (_buttonMode)
            {
                case ButtonMode.PressurePlate:
                    DeactivatePressurePlate();
                    break;
                case ButtonMode.Button:
                    break;
            }
        }

        private void DeactivatePressurePlate()
        {
            if (!_isActive) return;
            
            if (_numberOfSoftBodiesOnPlate > 0) return;
            
            foreach (IActivable obj in _activables)
            {
                obj.SwapState();
            }
            _spriteRenderer.sprite = _DeactiveSprite;
            _isActive = false;
        }


        private void ActivatePressurePlate()
        {
            if (_isActive) return;
            
            foreach (IActivable obj in _activables)
            {
                obj.SwapState();
            }
            _isActive = true;
            _spriteRenderer.sprite = _activeSprite;
        }

        public void ResetState()
        {
            _isActive = _isActiveStartState;
            _spriteRenderer.sprite = _isActive? _activeSprite : _DeactiveSprite;
            Restart(); // The parent restart method for reset his dictionary
        }
    }
}
