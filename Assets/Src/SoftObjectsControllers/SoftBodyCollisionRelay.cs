using System;
using MassInteraction;
using UnityEngine;

namespace SoftBodyControllers
{
    public class SoftBodyCollisionRelay : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D other)
        {
            IMass otherMass = other.gameObject.GetComponentInParent<IMass>();
            IMass myMass = this.gameObject.GetComponentInParent<IMass>();
            if (otherMass == null || myMass == null || otherMass == myMass)
            {
                Debug.Log("SoftBodyCollisionRelay::OnCollisionEnter2D: Collision rejected to pass forward for be the same object or invalid ones");
                return;
            }
            
            GetComponentInParent<SoftBody>()?.OnSoftBodyCollisionEnter?.Invoke(otherMass);
        }
    }
}