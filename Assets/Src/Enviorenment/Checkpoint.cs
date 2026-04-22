using System;
using System.Collections;
using MassInteraction;
using Player;
using UnityEngine;
[RequireComponent(typeof(SpringJoint2D))]
public class Checkpoint : MonoBehaviour, IActivable // Checkpoint is IActivable because it could activate other go
{
    [Header("Mass")] 
    [SerializeField] private float massCheck = 70f;
    
    [Header("Spring config")]
    [SerializeField] private float _animationTime = 1f;
    [SerializeField] private float _minSpringDistance = 0.1f;
    
    [SerializeField] private Vector2 _spawnOffset = new Vector2(0, 3);
    private bool _isActivated;
    private SpringJoint2D _springJoint2D;
    private float massDistanceRelation;
    private float springDistanceToMove;
    private float _defaultSpringDistance;
    
    private int _playerColliderCount;
    private IActivable[] _activables;
    
    private void Awake()
    {
        _springJoint2D = GetComponent<SpringJoint2D>();
        _defaultSpringDistance = _springJoint2D.distance;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !_isActivated && _playerColliderCount == 0)
        {
            massDistanceRelation = Mathf.Clamp01(other.gameObject.GetComponentInParent<PlayerController>().GetMass() / massCheck);
            massDistanceRelation *= _defaultSpringDistance;
            massDistanceRelation = _defaultSpringDistance - massDistanceRelation;
            springDistanceToMove = Mathf.Max(_minSpringDistance, massDistanceRelation);
            Activate();
        }
        _playerColliderCount++;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        _playerColliderCount--;

        if (_playerColliderCount == 0) DeActivate();
    }

    private void SetCheckpoint()
    {
        Debug.Log("SetCheckpoint");
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + _spawnOffset.y);
        CheckpointManager.Instance.SetCheckpoint(pos);
        
        _isActivated = true;
    }

    public void Activate()
    {
        StartCoroutine(nameof(AnimateJoint));
        
    }

    private IEnumerator AnimateJoint()
    {
        float time = 0;
        float baseDistance = _springJoint2D.distance;
        
        while (time <= 1)
        {
            _springJoint2D.distance = Mathf.Lerp(baseDistance, springDistanceToMove, time);
            time += Time.deltaTime / _animationTime;
            yield return null;
        }

        if (springDistanceToMove == _minSpringDistance)
        {
            _springJoint2D.distance = _minSpringDistance;
            SetCheckpoint();
        }
        else
        {
            springDistanceToMove = _defaultSpringDistance;
            StartCoroutine(nameof(AnimateJoint));
        }
    }

    public void DeActivate()
    {
        StopCoroutine(nameof(AnimateJoint));
        springDistanceToMove = _defaultSpringDistance;
        StartCoroutine(nameof(AnimateJoint));
    }

    public void SwapState()
    {
        throw new NotImplementedException();
    }
}
