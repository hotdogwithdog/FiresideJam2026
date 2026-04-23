using System;
using MassInteraction;
using SoftBodyControllers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, MassInteraction.IMass
    {
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _movementForce = 2f;

        [SerializeField] private Transform _playerAnchor; // Center of the slime

        private SlimeSoftBodyController _playerSoftBodyController;
        private Vector2 _movementDirection;

        [SerializeField] private float _mass = 100f;
        [SerializeField] private float _maxMass = 200f;
        
        // TODO: Remove this is just for testing the scale changes
        public bool goUp = true;

        void Start()
        {
            Inputs.InputReader.Instance.onMove += OnMove;
            Inputs.InputReader.Instance.onJump += OnJump;
            Inputs.InputReader.Instance.onInteract += OnInteract;
            _movementDirection = Vector2.zero;

            _playerSoftBodyController = GetComponent<SlimeSoftBodyController>();
            _playerSoftBodyController.OnSoftBodyCollisionEnter += OnSoftBodyCollision;
            _playerSoftBodyController.SetScale(Utilities.Maths.GetScaleFromMass(_mass));
        }

        private void OnSoftBodyCollision(IMass otherMass)
        {
            Debug.Log("Collision Reached");
            MassAbsortionManager.OnCollisionOfMass(this, otherMass);
        }

        private void OnInteract()
        {
            
        }

        private void FixedUpdate()
        {
            foreach (GameObject point in _playerSoftBodyController.Points)
            {
                float weight = Mathf.Clamp01(1f - (point.transform.position.y - _playerAnchor.position.y));
                point.GetComponent<Rigidbody2D>().AddForce(_movementDirection * (_movementForce * weight));
            }
        }

        private void OnJump()
        {
            // TODO: Make the jump just available if is on ground (must check the other nodes this script is set to the center point of the mass-spring system
            _playerSoftBodyController.AddForce(Vector2.up * _jumpForce, 0.5f, ForceMode2D.Impulse);
        }

        private void OnMove(Vector2 Direction)
        {
            _movementDirection = Direction;
        }

        #region IMass Implementation
        public float GetMass()
        {
            return _mass;
        }

        public void AbsorbMass(IMass other)
        {
            _mass = MathF.Min(_maxMass, _mass + other.GetMass());
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _playerSoftBodyController.SetScale(targetScale);
        }

        public void BeAbsorbed() { }
        public void ReduceMass(float amount)
        {
            _mass -= MathF.Max(0, _mass - amount);
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _playerSoftBodyController.SetScale(targetScale);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        // The Slime can not be absorbed

        #endregion
    }
}

