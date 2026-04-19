using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class BasicPlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _movementForce = 2f;

        [SerializeField] private Transform _playerAnchor; // Center of the slime

        private Rigidbody2D[] _points;
        private Vector2 _movementDirection;
        
        void Start()
        {
            Inputs.InputReader.Instance.onMove += OnMove;
            Inputs.InputReader.Instance.onJump += OnJump;
            _movementDirection = Vector2.zero;
            
            Rigidbody2D[] points = GetComponentsInChildren<Rigidbody2D>();
            _points = new Rigidbody2D[points.Length]; // Discard the center point
            for (int i = 0; i < points.Length; ++i)
            {
                _points[i] = points[i];
            }
        }
        
        private void FixedUpdate()
        {
            foreach (Rigidbody2D point in _points)
            {
                float weight = Mathf.Clamp01(1f - (point.transform.position.y - _playerAnchor.position.y));
                point.AddForce(_movementDirection * (_movementForce * weight));
            }
        }

        private void OnJump()
        {
            // TODO: Make the jump just available if is on ground (must check the other nodes this script is set to the center point of the mass-spring system
            foreach (Rigidbody2D point in _points)
            {
                if (point.transform.position.y < _playerAnchor.position.y)
                    point.AddForceY(_jumpForce, ForceMode2D.Impulse);
                else point.AddForceY(_jumpForce/2f, ForceMode2D.Impulse);
            }
        }

        private void OnMove(Vector2 Direction)
        {
            _movementDirection = Direction;
        }
    }
}

