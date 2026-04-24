using System;
using MassInteraction;
using SoftBodyControllers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, MassInteraction.IMass
    {
        [Header("Movement")]
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _movementForce = 2f;
        [SerializeField] private LayerMask _groundMask;
        
        [SerializeField] private Transform _playerAnchor; // Center of the slime
        
        private SlimeSoftBodyController _playerSoftBody;
        private Vector2 _movementDirection;

        [Header("Mass")]
        [SerializeField] private float _mass = 100f;
        [SerializeField] private float _maxMass = 200f;
        
        [Header("Mass throw")]
        [SerializeField] private GameObject _massBallPrefab;
        [SerializeField] private float _throwMassCantity = 5f;
        [SerializeField] private float _throwForce = 2f;
        [SerializeField] private float _timeOfIgnoreCollisionsWithThrowMass = 0.2f;
        
        #region publicInterface
        
        public Action<float> onMassChanged;
        public float MaxMass => _maxMass;
        
        
        #endregion

        private void Start()
        {
            Inputs.InputReader.Instance.onMove += OnMove;
            Inputs.InputReader.Instance.onJump += OnJump;
            Inputs.InputReader.Instance.onShootMass += OnShootMass;
            _movementDirection = Vector2.zero;

            _playerSoftBody = GetComponent<SlimeSoftBodyController>();
            _playerSoftBody.SetScale(Utilities.Maths.GetScaleFromMass(_mass));
            onMassChanged?.Invoke(_mass); // For the first set (the UI must be on the Awake to hear this)
        }

        private void OnShootMass(Vector2 mousePositionInScreenSpace)
        {
            if (_mass <= _throwMassCantity)
            {
                // TODO: Add a Sound of action rejected
                Debug.Log("PlayerController::OnShootMass: Mass insuficent, rejectMassThrow");
                return;
            }
            Vector3 mousePosWorldSpace = Camera.main.ScreenToWorldPoint(mousePositionInScreenSpace);
            Vector3 throwDirection = (mousePosWorldSpace - _playerAnchor.position).normalized;
            
            GameObject massBallGameObject = Instantiate(_massBallPrefab);
            massBallGameObject.transform.position = _playerAnchor.position + throwDirection * 3f;
            MassBall massBall = massBallGameObject.GetComponent<MassBall>();
            massBall.Init(_throwMassCantity);
            
            ReduceMass(_throwMassCantity);
            massBall.Throw(throwDirection * _throwForce, _playerSoftBody, _timeOfIgnoreCollisionsWithThrowMass);
        }

        private void FixedUpdate()
        {
            foreach (GameObject point in _playerSoftBody.Points)
            {
                float weight = Mathf.Clamp01(1f - (point.transform.position.y - _playerAnchor.position.y));
                point.GetComponent<Rigidbody2D>().AddForce(_movementDirection * (_movementForce * weight));
            }
        }

        private void OnJump()
        {
            bool isGrounded = Physics2D.Raycast(_playerAnchor.position, Vector2.down, _playerSoftBody.Scale * 0.5f + 0.5f, _groundMask);
            Debug.DrawLine(_playerAnchor.position, _playerAnchor.position + Vector3.down * (_playerSoftBody.Scale * 0.5f + 0.5f), Color.blue, 1f);
            if (!isGrounded) return;
            _playerSoftBody.AddForce(Vector2.up * _jumpForce, 1.5f, ForceMode2D.Impulse);
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
            Debug.Log("Player absorb");
            _mass = MathF.Min(_maxMass, _mass + other.GetMass());
            onMassChanged?.Invoke(_mass);
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _playerSoftBody.SetScale(targetScale);
        }

        public void BeAbsorbed() { }
        public bool IsBeingAbsorbed() { return false; }
        
        public void ReduceMass(float amount)
        {
            _mass = MathF.Max(0, _mass - amount);
            onMassChanged?.Invoke(_mass);
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _playerSoftBody.SetScale(targetScale);
        }

        

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        // The Slime can not be absorbed

        #endregion
    }
}

