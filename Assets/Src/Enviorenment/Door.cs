using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
    [SerializeField] private Transform _finalPosition;
    [SerializeField] private float _animationTime = 2f;
    [SerializeField] private bool _isOpen;

    private Vector2 _originPosition;
    private float _time = 1;

    private Coroutine _coroutine;

    private void Awake()
    {
        _originPosition = transform.position;
        if (_isOpen) transform.position = _finalPosition.position;
    }

    public void Activate()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(MoveDoor(_finalPosition.position));
        _isOpen = true;
    }

    private IEnumerator MoveDoor(Vector2 targetPosition)
    {
        float animationTime = _time * _animationTime;
        Debug.Log($"Animation time : {animationTime}, time {_time}");
        _time = 0;
        Vector2 originPosition = transform.position;
        while (_time <= 1)
        {
            transform.position = Vector2.Lerp(originPosition, targetPosition, _time);
            _time += Time.deltaTime / animationTime;
            yield return null;
        }
        transform.position = targetPosition;
        _time = 1;
    }

    public void DeActivate()
    {
        Debug.Log("Door deactivated");
        if (_coroutine != null)StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(MoveDoor(_originPosition));
        _isOpen = false;
    }

    public void SwapState()
    {
        if (!_isOpen)
        {
            Activate();
        }
        else
        {
            DeActivate();
        }
    }
}
