using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _deActiveSprite;
    
    [SerializeField] private GameObject[] _objectsToActivate;
    private int _playerColliderCount;
    private IActivable[] _activables;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (_playerColliderCount == 0) ActivatePressurePlate();
            
        _playerColliderCount++;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _playerColliderCount--;

        if (_playerColliderCount == 0) DeactivatePressurePlate();
    }

    private void DeactivatePressurePlate()
    {
        foreach (IActivable obj in _activables)
        {
            obj.DeActivate();
        }
        _spriteRenderer.sprite = _activeSprite;
    }


    private void ActivatePressurePlate()
    {
        foreach (IActivable obj in _activables)
        {
            obj.Activate();
        }
        _spriteRenderer.sprite = _deActiveSprite;
    }
}
