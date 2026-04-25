using System;
using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
    [SerializeField] private Vector2 _offset = new Vector2(0, -10);
    [SerializeField] private bool _isOpen;
    public void Activate()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y + _offset.y), 1 * Time.deltaTime);
        _isOpen = true;
    }

    public void DeActivate()
    {
        Debug.Log("Door deactivated");
        _isOpen = false;
    }

    public void SwapState()
    {
        if (_isOpen)
        {
            Activate();
        }
        else
        {
            DeActivate();
        }
    }
}
