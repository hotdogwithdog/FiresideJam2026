using System;
using SoftBodyControllers;
using UnityEngine;

namespace MassInteraction
{
    [RequireComponent(typeof(SoftBody))]
    public class MassBall : MonoBehaviour, IMass
    {
        [SerializeField] private float _mass = 5f;
        [SerializeField] private float _maxVisualScale = 3f;

        [SerializeField] private float _timeOfAbsortion = 0.5f;
        

        private SoftBody _softBody;
        private void Awake()
        {
            _softBody = GetComponent<SoftBody>();
            _softBody.OnSoftBodyCollisionEnter += OnCollision;
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
            
            Debug.Log($"MassBall::AbsorbMass: new Mass {_mass}");
            
            float targetScale = MathF.Min(_maxVisualScale, MathF.Sqrt(_mass));
            
            _softBody.SetScale(targetScale);
        }

        public void BeAbsorbed()
        {
            Debug.Log("MassBall::BeAbsorbed: Reached absorbing");
            Destroy(_softBody.gameObject);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}