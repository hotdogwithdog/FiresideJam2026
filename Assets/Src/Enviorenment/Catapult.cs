using System;
using System.Collections;
using UnityEngine;

public class Catapult : MonoBehaviour, IActivable
{
    [Header("Settings")]
    [SerializeField] private float motorSpeed;
    [SerializeField] private float motorForce;
    
    [Header("Timer")]
    [SerializeField] private float _catapultTimer = 1f;

    #region Hinge
        private HingeJoint2D _hinge;
        private JointMotor2D _motor;
    #endregion

    private float _resetTimer = 2f;
    private bool _isTimerRunning;

    private int _playerColliderCount;

    private void Awake()
    {
        _hinge = GetComponent<HingeJoint2D>();
        _motor = _hinge.motor;
    }

    private void OnCollisionEnter2D(Collision2D other) //TODO: onexit limpiar contador de bolas
    {
        if (!other.collider.CompareTag("Player")) return;

        if (_playerColliderCount == 0)
        {
            _isTimerRunning = true;
            StartCoroutine(Launch());
        }
            Debug.Log(other.collider.name);
        _playerColliderCount++;
    }

    private IEnumerator Launch()
    {
        yield return new WaitForSeconds(_catapultTimer);
        Activate();
    }

    private IEnumerator ResetCatapult()
    {
        yield return new WaitForSeconds(_resetTimer);
        _motor.motorSpeed = -motorSpeed;
        _motor.maxMotorTorque = motorForce;
        _hinge.motor = _motor;
        _hinge.useMotor = true;
        _playerColliderCount = 0;
    }

    public void Activate()
    {
       _hinge.useMotor = false;
       _motor.motorSpeed = motorSpeed;
       _motor.maxMotorTorque = motorForce;
       _hinge.motor = _motor;
       _hinge.useMotor = true;
       
       StartCoroutine(ResetCatapult());
    }
    
    public void DeActivate()
    {
        throw new System.NotImplementedException();
    }

    public void SwapState()
    {
        throw new System.NotImplementedException();
    }


}
