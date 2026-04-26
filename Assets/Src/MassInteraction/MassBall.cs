using System;
using System.Collections;
using Audio;
using SoftBodyControllers;
using UnityEngine;

namespace MassInteraction
{
    [RequireComponent(typeof(SoftBody))]
    public class MassBall : MonoBehaviour, IMass
    {
        [Header("Audio")][SerializeField] private AudioClip _absorbAudioClip;
        [SerializeField] private float _mass = 5f;

        private bool _isBeingAbsorbed = false;

        private SoftBody _softBody;

        private void Awake()
        {
            _softBody = GetComponent<SoftBody>();
            _softBody.OnSoftBodyCollisionEnter += OnCollision;
        }
        public void Init(float mass)
        {
            _mass = mass;
        }
        private void Start()
        {
            _softBody.SetScale(Utilities.Maths.GetScaleFromMass(_mass));
        }

        public void Throw(Vector2 force, SoftBody softBodyToIgnoreWhileThrowing, float timeOfIgnore)
        {
            IgnoreColliders(softBodyToIgnoreWhileThrowing, true);
            _softBody.AddForce(force, 1f, ForceMode2D.Impulse);
            StartCoroutine(ReenableCollision(softBodyToIgnoreWhileThrowing, timeOfIgnore));
        }

        // The time is in seconds
        private IEnumerator ReenableCollision(SoftBody softBodyToReenable, float timeToReenable)
        {
            yield return new WaitForSeconds(timeToReenable);
            
            IgnoreColliders(softBodyToReenable, false);
        }

        private void IgnoreColliders(SoftBody softBodyToIgnore, bool ignore)
        {
            foreach (GameObject point in softBodyToIgnore.Points)
            {
                Collider2D colliderToIgnore = point.GetComponent<Collider2D>();

                if (colliderToIgnore == null) continue;
                
                foreach (GameObject myPoint in _softBody.Points)
                {
                    Collider2D myCollider = myPoint.GetComponent<Collider2D>();
                    if (myCollider == null) continue;
                    Physics2D.IgnoreCollision(colliderToIgnore, myCollider, ignore);
                }
            }
        }

        private void OnCollision(IMass otherMass)
        {
            if (_isBeingAbsorbed) return;
            MassAbsortionManager.OnCollisionOfMass(this, otherMass);
        }

        public float GetMass()
        {
            return _mass;
        }

        public void AbsorbMass(IMass other)
        {
            _mass += other.GetMass();
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _softBody.SetScale(targetScale);
            
            _isBeingAbsorbed = false;
            
            AudioManager.Instance.PlayOneShot2D(_absorbAudioClip,1f);
        }

        public void BeAbsorbed()
        {
            Debug.Log("MassBall::BeAbsorbed: Reached absorbing");
            _isBeingAbsorbed = true;
            Destroy(_softBody.gameObject);
        }

        public void ReduceMass(float amount)
        {
            _mass = MathF.Max(0f, _mass - amount);
            
            float targetScale = Utilities.Maths.GetScaleFromMass(_mass);
            
            _softBody.SetScale(targetScale);
        }

        public bool IsBeingAbsorbed()
        {
            return _isBeingAbsorbed;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}