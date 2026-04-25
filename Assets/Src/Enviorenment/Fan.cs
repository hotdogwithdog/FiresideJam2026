using System;
using Enviorenment;
using MassInteraction;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Animator))]
public class Fan : ASoftBodyInteract, IActivable
{
    
    [SerializeField] private float _massToReduce = 0.5f;
    [SerializeField] private float _fanForce = 3f;
    
    [SerializeField] private bool _isActivated = true;
    private ParticleSystem _particleSystem;
    private Animator _animator;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isActivated) return;
        
        if (other.CompareTag("Player") || other.CompareTag("MassBall"))
        {
            PushPlayer(other.gameObject);
        }
    }

    public void Activate()
    {
        _particleSystem.Play();
        _animator.SetBool("isActive", true);
        _isActivated = true;
    }

    private void PushPlayer(GameObject softBody)
    {
        Vector2 direction = softBody.transform.position - transform.position;
        softBody.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _fanForce, ForceMode2D.Force);
    }

    public void DeActivate()
    {
        _particleSystem.Stop();
        _animator.SetBool("isActive", false);
        _isActivated = false;
    }

    public void SwapState()
    {
        if (_isActivated)
        {
            DeActivate();
        }
        else
        {
            Activate();
        }
    }

    protected override void OnSoftBodyEntered(IMass softBodyMass) { }

    protected override void OnSoftBodyStay(IMass softBodyMass)
    {
        if (!_isActivated) return;
        softBodyMass.ReduceMass(_massToReduce);
    }

    protected override void OnSoftBodyExit(IMass softBodyMass) { }
}
