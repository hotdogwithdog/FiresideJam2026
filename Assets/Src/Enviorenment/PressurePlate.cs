using System;
using Enviorenment;
using MassInteraction;
using UnityEngine;

public class PressurePlate : ASoftBodyInteract
{
    [SerializeField] private ButtonMode _buttonMode = ButtonMode.PressurePlate;
    [SerializeField] private GameObject[] _objectsToActivate;
    private IActivable[] _activables;
    
    [Header("Sprites")]
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _DeactiveSprite;
    
    
    [SerializeField] private bool _isActive;

    private enum ButtonMode
    {
        PressurePlate,
        Button
    }

    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _activables = new IActivable[_objectsToActivate.Length];
        for (int i = 0; i < _objectsToActivate.Length; i++)
        {
            _activables[i] = _objectsToActivate[i].GetComponent<IActivable>();
        }
    }
    
    protected override void OnSoftBodyEntered(IMass softBodyMass)
    {
        if (_isActive) return;
        ActivatePressurePlate();
    }

    protected override void OnSoftBodyStay(IMass softBodyMass)
    {
       
    }

    protected override void OnSoftBodyExit(IMass softBodyMass)
    {
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
        foreach (IActivable obj in _activables)
        {
            obj.SwapState();
        }
        _spriteRenderer.sprite = _DeactiveSprite;
        _isActive = false;
    }


    private void ActivatePressurePlate()
    {
        foreach (IActivable obj in _activables)
        {
            obj.SwapState();
        }
        _isActive = true;
        _spriteRenderer.sprite = _activeSprite;
    }
}
