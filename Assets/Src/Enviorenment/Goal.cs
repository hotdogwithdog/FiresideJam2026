using System.Collections;
using Player;
using UnityEngine;

public class Goal : MonoBehaviour, IActivable
{

    [SerializeField] private float _massCheck = 70f;

    [Header("Spring config")] [SerializeField]
    private float _animationTime = 1f;

    [SerializeField] private float _minSpringDistance = 0.1f;

    private PlayerController _playerController;

    private bool _isActivated = false;
    private SpringJoint2D _springJoint2D;
    private float massDistanceRelation;
    private float springDistanceToMove;
    private float _defaultSpringDistance;

    private int _playerColliderCount = 0;

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
            _playerController = other.gameObject.GetComponentInParent<PlayerController>();
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

    private void Win()
    {
        //MenuManager.Instance.SetState(new EndLevel());
        Debug.Log("Win");
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
            Win();
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
