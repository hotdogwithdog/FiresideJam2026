using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicPlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _movementForce = 2f;


        private Rigidbody2D _rigidBody;

        private Rigidbody2D[] _points;
        
        private Vector2 _movementDirection;
        
        void Start()
        {
            Inputs.InputReader.Instance.onMove += OnMove;
            Inputs.InputReader.Instance.onJump += OnJump;
            _rigidBody = GetComponent<Rigidbody2D>();
            _movementDirection = Vector2.zero;
            
            Rigidbody2D[] points = GetComponentsInChildren<Rigidbody2D>();
            _points = new Rigidbody2D[points.Length - 1]; // Discard the center point
            int index = 0;
            for (int i = 0; i < points.Length; ++i)
            {
                if (_rigidBody == points[i])
                {
                    continue;
                }
                _points[index++] = points[i];
            }
        }
        
        private void FixedUpdate()
        {
            foreach (Rigidbody2D point in _points)
            {
                point.linearVelocity = point.linearVelocity + Time.fixedDeltaTime * _movementForce * _movementDirection;
                point.position = point.position + point.linearVelocity * Time.fixedDeltaTime;
            }
            _rigidBody.linearVelocity = _rigidBody.linearVelocity + Time.fixedDeltaTime * _movementForce * _movementDirection;
            
            _rigidBody.position = _rigidBody.position + Time.fixedDeltaTime * _rigidBody.linearVelocity;
        }

        private void OnJump()
        {
            // TODO: Make the jump just available if is on ground (must check the other nodes this script is set to the center point of the mass-spring system
            foreach (Rigidbody2D point in _points)
            {
                point.AddForceY(_jumpForce / _points.Length, ForceMode2D.Impulse);
            }
            _rigidBody.AddForceY(_jumpForce / _points.Length, ForceMode2D.Impulse);
        }

        private void OnMove(Vector2 Direction)
        {
            _movementDirection = Direction;
        }
    }
}

