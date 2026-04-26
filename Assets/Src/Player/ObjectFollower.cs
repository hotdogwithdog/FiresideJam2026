using System;
using UnityEngine;

namespace Player
{
    public class ObjectFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _offsetY; 

        private void Update()
        {
            transform.position = new Vector3(_target.position.x, _target.position.y + _offsetY, _target.position.z);
        }
    }
}