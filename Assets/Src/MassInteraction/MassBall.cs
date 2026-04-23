using System;
using SoftBodyControllers;
using UnityEngine;

namespace MassInteraction
{
    [RequireComponent(typeof(SoftBody))]
    public class MassBall : MonoBehaviour, IMass
    {
        [SerializeField] private float _mass = 5f;

        [SerializeField] private float _timeOfAbsortion = 0.5f;
        

        private SoftBody _softBody;
        private void Awake()
        {
            _softBody = GetComponent<SoftBody>();
            _softBody.OnSoftBodyCollisionEnter += OnCollision;
            _softBody.SetScale(Utilities.Maths.GetScaleFromMass(_mass));
        }

        private void OnCollision(IMass otherMass)
        {
            Debug.Log("Collision Reached");
            MassAbsortionManager.OnCollisionOfMass(this, otherMass);
        }

        public float GetMass()
        {
            return _mass;
        }

        public void AbsorbMass(IMass other)
        {
            _mass += other.GetMass();
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _softBody.SetScale(targetScale);
        }

        public void BeAbsorbed()
        {
            Debug.Log("MassBall::BeAbsorbed: Reached absorbing");
            Destroy(_softBody.gameObject);
        }

        public void ReduceMass(float amount)
        {
            _mass = MathF.Max(0f, amount);
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _softBody.SetScale(targetScale);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}