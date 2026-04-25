using System;
using Player;
using SoftBodyControllers;
using UnityEngine;
public class CameraController : MonoBehaviour
{ 
    [SerializeField]private Vector2 _offset = new Vector2(0f, 2f);
    [SerializeField] private float _smoothTime = 0.25f;
    private Vector3 _velocity = Vector3.zero;

    private Transform _target;


    private void Awake()
    {
        _target = FindFirstObjectByType<PlayerController>().GetComponent<SoftBody>().Anchor;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _target.position + new Vector3(_offset.x, _offset.y, 0);
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }
}
