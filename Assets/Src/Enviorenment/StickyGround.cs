using System;
using Enviorenment;
using MassInteraction;
using UnityEngine;

public class StickyGround : ASoftBodyInteract
{
    [SerializeField] private float _massToReduce = 0.5f;


    protected override void OnSoftBodyEntered(IMass softBodyMass) { }

    protected override void OnSoftBodyStay(IMass softBodyMass)
    {
        softBodyMass.ReduceMass(_massToReduce);
    }

    protected override void OnSoftBodyExit(IMass softBodyMass) { }
}
