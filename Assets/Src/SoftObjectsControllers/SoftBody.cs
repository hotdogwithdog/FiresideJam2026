using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SoftBodyControllers
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class SoftBody : MonoBehaviour
    {
        protected struct PointJointsDistances
        {
            public int index;
            public float[] baseDistances;
            public SpringJoint2D[] springJoints;
        }


        [Header("Utilities")]
        // This transform is updated each frame (LateUpdate) with the center of the softbody
        [SerializeField]
        protected Transform _anchor;

        [Header("Softbody Configuration")]
        // Factor to the size of the tangents of the spline that has the spriteShape
        [SerializeField]
        protected float _tangentFactor = 0.25f;

        // More pressure means more force to preserve the shape of the mass-spring system
        [Range(0f, 400f)] [SerializeField] protected float _pressureStrenght = 40f;

        [Header("Spring-Mass system")]
        // The original points that this has no matters, just the configuration of the component and his profile
        protected SpriteShapeController _spriteShapeController;

        // If you are in a child object that generates the points on fly this is not mean to fill it in editor, if not they are nodes of the softbody (they must have rigidBody2D)
        [SerializeField] protected GameObject[] _points;
        [SerializeField] protected float _scale = 1f;
        
        public float Scale {get { return _scale; }}
        public GameObject[] Points { get { return _points; } }


        protected List<PointJointsDistances> _pointJointsDistances;
        protected float[] _pointsColliderRaiusBase;

        protected float _targetAreaBase;
        protected float _targetArea;
        

        #region PublicInterface
        
        public Action<MassInteraction.IMass> OnSoftBodyCollisionEnter;
        public Transform Anchor => _anchor;

        public void Teleport(Vector2 position)
        {
            foreach (GameObject point in _points)
            {
                Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
                
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.position = rb.position - (new Vector2(_anchor.position.x, _anchor.position.y) - position);
            }
        }
        
        public Vector2 GetAverageVelocity()
        {
            Vector2 sum = Vector2.zero;

            foreach (GameObject point in _points)
            {
                sum += point.GetComponent<Rigidbody2D>().linearVelocity;
            }
            
            return sum / _points.Length;
        }

        public void AddForce(Vector2 force, float scaleForFarPoints, ForceMode2D forceMode)
        {
            if (force == Vector2.zero)
            {
                Debug.LogWarning("SoftBody::AddForce: force is (0, 0)");
                return;
            }
            
            Vector2 forceNormal = new Vector2(force.y, -force.x); // Clock-wise perpendicular vector
            float angle = -MathF.Atan2(forceNormal.y, forceNormal.x); // Angle between the normal and the right vector in radians (swap the direction in preparation to the rotation that is counterclock-wise
            Vector2 rotatedAnchor = Utilities.Maths.Rotate2D(angle, _anchor.transform.position);
            Debug.DrawLine(_anchor.transform.position - new Vector3(forceNormal.x, forceNormal.y, 0f) * 5f, _anchor.transform.position + new Vector3(forceNormal.x, forceNormal.y, 0f) * 5f,
                Color.blueViolet, 1f);
            foreach (GameObject point in _points)
            {
                Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
                
                //Vector2 movedPoint = point.transform.position - _anchor.transform.position; // put the anchor at the origin for preparation to the rotation the compare must be with 0
                // But we can just rotate the point and the anchor and compare with it, without moving because we just care if is in one side and even if the anchor is not at the center
                // of the space coordinates it stay horizontally and can done the check that we want so that will be on the code for simplicity

                if (Utilities.Maths.Rotate2D(angle, point.transform.position).y < rotatedAnchor.y)
                {
                    rb.AddForce(force, forceMode);
                    Utilities.Debug.DrawPoint(point.transform.position, point.GetComponent<CircleCollider2D>().radius + 0.05f, Color.red);
                }
                else
                {
                    rb.AddForce(force * scaleForFarPoints, forceMode);
                    Utilities.Debug.DrawPoint(point.transform.position, point.GetComponent<CircleCollider2D>().radius + 0.05f, Color.green);
                }
            }
        }
        #endregion
        
        protected void Awake()
        {
            _spriteShapeController = GetComponent<SpriteShapeController>();
            CreatePoints(); // Just if the child overrides the function it will do anything
            InitPointJointsDistances();
            _targetAreaBase = CalculateArea();
            _targetArea = _targetAreaBase;
            //SetScale(_scale, true);
            MatchPointsOfSpriteShape();
            UpdateSpline();
        }

        protected virtual void CreatePoints() { }

        protected void InitPointJointsDistances()
        {
            // at least that space but probably it need to reallocate for more space the alternative is one previous foreach that counts how many springJoints have the object
            _pointJointsDistances = new List<PointJointsDistances>(_points.Length);
            _pointsColliderRaiusBase =  new float[_points.Length];
            for (int i = 0; i < _points.Length; ++i)
            {
                SpringJoint2D[] joints = _points[i].gameObject.GetComponents<SpringJoint2D>();
                PointJointsDistances pointJointsDistances = new PointJointsDistances();
                pointJointsDistances.index = i;
                pointJointsDistances.baseDistances = new float[joints.Length];
                pointJointsDistances.springJoints = joints;

                for (int j = 0; j < joints.Length; ++j)
                {
                    pointJointsDistances.baseDistances[j] = joints[j].distance;
                }
                
                _pointJointsDistances.Add(pointJointsDistances);
                _pointsColliderRaiusBase[i] = _points[i].GetComponent<CircleCollider2D>().radius;
            }
        }

        public void SetScale(float newScale, bool forceUpdate = false)
        {
            if (forceUpdate == false && _scale == newScale) return;
            _scale = newScale;

            if (_points.Length != _pointJointsDistances.Count)
            {
                Debug.LogWarning("SoftBody::SetScale: The points do not match correctly recalculing with the actual distances");
                InitPointJointsDistances();
            }

            for (int i = 0; i < _pointJointsDistances.Count; ++i)
            {
                for (int j = 0; j < _pointJointsDistances[i].springJoints.Length; ++j)
                {
                    _pointJointsDistances[i].springJoints[j].distance = _pointJointsDistances[i].baseDistances[j] * _scale;
                }
            }

            for (int i = 0; i < _points.Length; ++i)
            {
                _points[i].GetComponent<CircleCollider2D>().radius = _pointsColliderRaiusBase[i] * _scale;
            }
            
            _targetArea = _targetAreaBase * _scale * _scale; // Is a surface
        }

        protected float CalculateArea()
        {
            float area = 0f;

            for (int i = 0; i < _points.Length; ++i)
            {
                Vector2 A = _points[i].transform.position;
                Vector2 B = _points[(i + 1) % _points.Length].transform.position;

                area += (A.x * B.y) - (B.x * A.y);
            }

            return MathF.Abs(area) * 0.5f;
        }

        protected void ApplyPressure()
        {
            if (_pressureStrenght == 0f) return;

            float currentArea = CalculateArea();

            float pressure = (_targetArea - currentArea) / _targetArea;
            for (int i = 0; i < _points.Length; ++i)
            {
                Rigidbody2D rb = _points[i].GetComponent<Rigidbody2D>();

                Vector2 prev = _points[(i - 1 + _points.Length) % _points.Length].transform.position;
                Vector2 next = _points[(i + 1) % _points.Length].transform.position;

                Vector2 edge = next - prev;

                Vector2 normal = new Vector2(-edge.y, edge.x).normalized;

                Vector2 force = normal * (pressure * _pressureStrenght);

                rb.AddForce(-force);
            }
        }

        private void MatchPointsOfSpriteShape()
        {
            _spriteShapeController.spline.Clear();
            _spriteShapeController.spline.isOpenEnded = false;

            for (int i = 0; i < _points.Length; ++i)
            {
                _spriteShapeController.spline.InsertPointAt(i, _points[i].transform.localPosition);
                _spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            }
        }

        private void FixedUpdate()
        {
            ApplyPressure();
        }

        private void LateUpdate()
        {
            UpdateSpline();
            _spriteShapeController.RefreshSpriteShape();
        }

        private void UpdateSpline()
        {
            Vector2 center = GetCenter(false);
            _anchor.position = GetCenter(true);
            for (int i = 0; i < _points.Length; ++i)
            {
                Vector2 vertex = _points[i].transform.localPosition;

                Vector2 pointToCenter = (center - vertex).normalized;

                float colliderRadius = _points[i].gameObject.GetComponent<CircleCollider2D>().radius;
                try
                {
                    _spriteShapeController.spline.SetPosition(i, vertex - pointToCenter * colliderRadius);
                }
                catch
                {
                    Debug.LogWarning("SoftBody::UpdateSpline: Points of the spline to close adding offset");
                    _spriteShapeController.spline.SetPosition(i, vertex - pointToCenter * (colliderRadius + 0.5f));
                }

                Vector2 prev = _points[(i - 1 + _points.Length) % _points.Length].transform.localPosition;
                Vector2 prevToCenter = (center - prev).normalized;
                Vector2 prevEdge = prev - prevToCenter * _points[(i - 1 + _points.Length) % _points.Length]
                    .GetComponent<CircleCollider2D>().radius;

                Vector2 next = _points[(i + 1) % _points.Length].transform.localPosition;
                Vector2 nextToCenter = (center - next).normalized;
                Vector2 nextEdge = next - nextToCenter *
                    _points[(i + 1) % _points.Length].GetComponent<CircleCollider2D>().radius;

                Vector2 rightTangent = (nextEdge - prevEdge).normalized;

                float lenght = (prev - vertex).magnitude + (next - vertex).magnitude;
                rightTangent *= lenght * _tangentFactor;

                _spriteShapeController.spline.SetLeftTangent(i, -rightTangent);
                _spriteShapeController.spline.SetRightTangent(i, rightTangent);
            }
        }

        private Vector2 GetCenter(bool isGlobalSpace)
        {
            Vector2 sum = Vector2.zero;

            for (int i = 0; i < _points.Length; ++i)
            {
                if (isGlobalSpace)
                    sum += (Vector2)_points[i].transform.position;
                else sum += (Vector2)_points[i].transform.localPosition;
            }

            return sum / _points.Length;
        }
        
        protected void ClearPointsObjects()
        {
            if (_points != null)
            {
                for (int i = 0; i < _points.Length; ++i)
                {
                    #if UNITY_EDITOR
                        DestroyImmediate(_points[i]);
                    #else
                        Destroy(_points[i]);
                    #endif
                }
            }
            _points = null;
        }

        private void OnDestroy()
        {
            if (_points != null)
            {
                ClearPointsObjects();
            }
        }
    }
}