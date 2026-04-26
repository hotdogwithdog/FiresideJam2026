using System.Collections.Generic;
using MassInteraction;
using UnityEngine;

namespace Environment
{
    public abstract class ASoftBodyInteract: MonoBehaviour
    {
        protected Dictionary<int, SoftBodyData> _softBodyDataDictionary = new Dictionary<int, SoftBodyData>();
        protected class SoftBodyData
        {
            public IMass iMass;
            public int count;

            public SoftBodyData(IMass iMass, int count)
            {
                this.iMass = iMass;
                this.count = count;
            }
        }

        protected void Restart()
        {
            _softBodyDataDictionary.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IMass mass = other.gameObject.GetComponentInParent<IMass>(); 
            if (mass == null) return;

            SoftBodyEnter(mass);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            IMass mass = other.gameObject.GetComponentInParent<IMass>(); 
            if (mass == null) return;

            SoftBodyExit(mass);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            IMass mass = other.collider.GetComponentInParent<IMass>(); 
            if (mass == null) return;

            SoftBodyEnter(mass);
        }

        private void SoftBodyEnter(IMass mass)
        {
            if (!_softBodyDataDictionary.ContainsKey(mass.GetGameObject().GetInstanceID()))
            {
                SoftBodyData data =  new SoftBodyData(mass, 0);
                _softBodyDataDictionary.Add(mass.GetGameObject().GetInstanceID(), data);
                
            }
            SoftBodyData collisionData = _softBodyDataDictionary[mass.GetGameObject().GetInstanceID()];
            
            if (collisionData.count == 0)
            {
                OnSoftBodyEntered(collisionData.iMass);
            }
            collisionData.count++;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            IMass mass = other.collider.GetComponentInParent<IMass>(); 
            if (mass == null) return;
            
            SoftBodyExit(mass);
        }

        private void SoftBodyExit(IMass mass)
        {
            SoftBodyData collisionData = _softBodyDataDictionary[mass.GetGameObject().GetInstanceID()];
            collisionData.count--;
            
            if (collisionData.count == 0)
            {
                Debug.Log("Ejecuto OnExit");
                OnSoftBodyExit(collisionData.iMass);
                _softBodyDataDictionary.Remove(mass.GetGameObject().GetInstanceID());
            }
        }

        private void Update()
        {
            
            foreach (SoftBodyData data in _softBodyDataDictionary.Values)
            {
                if (data.count <= 0) continue;
                
                // The MassBall can be absorb inside the collider/trigger
                if (data.iMass == null) continue; // The Memory of the ball absorbed inside the collider is not free
                
                OnSoftBodyStay(data.iMass);
            }
        }

        protected abstract void OnSoftBodyEntered(IMass softBodyMass);
        protected abstract void OnSoftBodyStay(IMass softBodyMass);
        protected abstract void OnSoftBodyExit(IMass softBodyMass);
        
        
    }
}