using System;
using Enviorenment;
using MassInteraction;
using UnityEngine;

public class Fan : ASoftBodyInteract, IActivable
{
    
    [SerializeField] private float _massToReduce = 0.5f;
    [SerializeField] private float _fanForce = 3f;
    
    [SerializeField] private bool _isActivated = true;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isActivated) return;
        
        if (other.CompareTag("Player"))
        {
            PushPlayer(other.gameObject);
        }
    }

    public void Activate()
    {
        _isActivated = true;
    }

    private void PushPlayer(GameObject softBody)
    {
        Vector2 direction = softBody.transform.position - transform.position;
        softBody.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _fanForce, ForceMode2D.Force);
    }

    public void DeActivate()
    {
        _isActivated = false;
    }

    public void SwapState()
    {
        _isActivated = !_isActivated;
    }

    protected override void OnSoftBodyEntered(IMass softBodyMass) { }

    protected override void OnSoftBodyStay(IMass softBodyMass)
    {
        softBodyMass.ReduceMass(_massToReduce);
    }

    protected override void OnSoftBodyExit(IMass softBodyMass) { }
}
