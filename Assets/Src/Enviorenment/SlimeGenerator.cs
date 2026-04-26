using System;
using System.Collections;
using Audio;
using MassInteraction;
using UnityEngine;

namespace Environment
{
    public class SlimeGenerator : MonoBehaviour, IActivable, IReseteable
    {
        
        [Header("Mass")]
        [SerializeField] private GameObject _massBallPrefab;
        [SerializeField] private float _massQuantity = 5f;
        [SerializeField] private float _interval = 2f;
        [SerializeField] private int _massBallQuantity = 5;
        private int _massBallQuantityStartState;

        [SerializeField] private Transform _spawnPoint;
        
        [SerializeField] private bool _isActive;
        private bool _isActiveStartState;
        [Header("Audio")]
        [SerializeField] private AudioClip _activateAudioClip;
        private void Start()
        {
            _isActiveStartState = _isActive;
            _massBallQuantityStartState = _massBallQuantity;
            
            if (_isActive) Activate();
            else DeActivate();
        }

        public void Activate()
        {
            
           StartCoroutine(nameof(SpawnBall));
           _isActive = true;
        }

        private IEnumerator SpawnBall()
        {
            while (_massBallQuantity > 0)
            {
                GameObject massBallGameObject = Instantiate(_massBallPrefab);
                massBallGameObject.transform.position = _spawnPoint.position;
                MassBall massBall = massBallGameObject.GetComponent<MassBall>();
                massBall.Init(_massQuantity);
                AudioManager.Instance.PlayOneShot2D(_activateAudioClip,1f);
                yield return new WaitForSeconds(_interval);
                _massBallQuantity--;
            }
        }

        public void DeActivate()
        {
           StopCoroutine(nameof(SpawnBall));
           _isActive = false;
        }

        public void SwapState()
        {
            if (_isActive) DeActivate();
            else Activate();
        }

        public void ResetState()
        {
            _isActive = _isActiveStartState;
            _massBallQuantity = _massBallQuantityStartState;
            
            if (_isActive)  Activate();
            else DeActivate();
        }
    }
}

