using System;
using MassInteraction;
using UnityEngine;

namespace Environment
{
    public class StickyGround : ASoftBodyInteract, IReseteable
    {
        [SerializeField] private float _massToReduce = 0.5f;


        protected override void OnSoftBodyEntered(IMass softBodyMass) { }

        protected override void OnSoftBodyStay(IMass softBodyMass)
        {
            softBodyMass.ReduceMass(_massToReduce);
        }

        protected override void OnSoftBodyExit(IMass softBodyMass) { }
        public void ResetState()
        {
            Restart(); // Need to call this for not reduce the mass of the player if he restart on the stickyGround
        }
    }
}
