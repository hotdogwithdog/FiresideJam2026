using System;
using System.Collections;
using MassInteraction;
using UnityEngine;

public class SlimeGenerator : MonoBehaviour, IActivable
{
    
    [Header("Mass")]
    [SerializeField] private GameObject _massBallPrefab;
    [SerializeField] private float _massCuantity = 5f;
    [SerializeField] private float _interval = 2f;
    [SerializeField] private int _massBallCuantity = 5;

    [SerializeField] private Transform _spawnPoint;
    
    [SerializeField] private bool _isActive;

    private void Start()
    {
        if (_isActive)
        {
            Activate();
        }
        else
        {
            DeActivate();
        }
    }

    public void Activate()
    {
        
       StartCoroutine(nameof(SpawnBall));
       _isActive = true;
    }

    private IEnumerator SpawnBall()
    {
        while (_massBallCuantity > 0)
        {
            GameObject massBallGameObject = Instantiate(_massBallPrefab);
            massBallGameObject.transform.position = _spawnPoint.position;
            MassBall massBall = massBallGameObject.GetComponent<MassBall>();
            massBall.Init(_massCuantity);
            yield return new WaitForSeconds(_interval);
            _massBallCuantity--;
        }
    }

    public void DeActivate()
    {
       StopCoroutine(nameof(SpawnBall));
       _isActive = false;
    }

    public void SwapState()
    {
        if (_isActive)
        {
            DeActivate();
        }
        else
        {
            Activate();
        }
    }
}
