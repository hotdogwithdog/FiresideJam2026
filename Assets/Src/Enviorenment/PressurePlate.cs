using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    private int _playerColliderCount;
    private IActivable[] _activables;

    private void Start()
    {
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
    }


    private void ActivatePressurePlate()
    {
        foreach (IActivable obj in _activables)
        {
            obj.Activate();
        }
    }
}
