using System.Collections;
using Audio;
using Player;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(SpringJoint2D))]
    public class Checkpoint : MonoBehaviour, IActivable // Checkpoint is IActivable because it could activate other go
    {
        [Header("Checkpoint Config")] 
        [SerializeField] private int _index;
        [SerializeField] private float _massCheck = 70f;
        [SerializeField] private Transform _spawnPoint; // 
        
        [Header("Spring config")]
        [SerializeField] private float _animationTime = 1f;
        [SerializeField] private float _minSpringDistance = 0.1f;
    
        private PlayerController _playerController;
        
        private bool _isActivated = false;
        private SpringJoint2D _springJoint2D;
        private float massDistanceRelation;
        private float springDistanceToMove;
        private float _defaultSpringDistance;
        
        private int _playerColliderCount = 0;
        private IActivable[] _activables;
        
        [Header("Audio")]
        [SerializeField] private AudioClip _activateAudioClip;
        
        private void Awake()
        {
            _springJoint2D = GetComponent<SpringJoint2D>();
            _defaultSpringDistance = _springJoint2D.distance;
        }
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            if (_playerColliderCount == 0 && !_isActivated)
            {
                Debug.Log("Player collision");
                _playerController =  other.gameObject.GetComponentInParent<PlayerController>();
                massDistanceRelation = Mathf.Clamp01(_playerController.GetMass() / _massCheck);
                massDistanceRelation *= _defaultSpringDistance;
                massDistanceRelation = _defaultSpringDistance - massDistanceRelation;
                springDistanceToMove = Mathf.Max(_minSpringDistance, massDistanceRelation);
                Activate();
            }
            _playerColliderCount++;
        }
    
        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            _playerColliderCount--;
    
            if (_playerColliderCount == 0 && !_isActivated) DeActivate();
        }
    
        private void SetCheckpoint()
        {
            Debug.Log("SetCheckpoint");
            CheckpointManager.CheckpointData data;
            data.checkpointIndex = _index;
            data.position = _spawnPoint.position;
            data.mass = _playerController.GetMass();
            CheckpointManager.Instance.SetCheckpoint(data);

            _isActivated = true;
            
            AudioManager.Instance.PlayOneShot2D(_activateAudioClip,4f);
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
        }
    }
}


