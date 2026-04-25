using System;
using UnityEngine;

public class Fan : MonoBehaviour, IActivable
{
    [SerializeField] private float _fanForce = 3f;
    private GameObject _player;
    
    [SerializeField] private bool _isActivated = true;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isActivated) return;
        
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
            PushPlayer();
        }
    }

    public void Activate()
    {
        _isActivated = true;
    }

    private void PushPlayer()
    {
        Vector2 direction = _player.transform.position - transform.position;
        _player.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _fanForce, ForceMode2D.Force);
    }

    public void DeActivate()
    {
        _isActivated = false;
    }

    public void SwapState()
    {
        _isActivated = !_isActivated;
    }
}
