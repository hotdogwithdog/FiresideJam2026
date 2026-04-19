using System;
using UnityEngine;
using UnityEngine.U2D;

namespace SoftBodyControllers
{
    public class SoftBody : MonoBehaviour
    {
        [SerializeField] protected int _numberOfVertices = 16;
        [SerializeField] protected float _radius = 2f;
        [SerializeField] protected Transform _anchor;
        [SerializeField] protected float _pointRadius = 0.25f;
        [SerializeField] protected float _tangentFactor = 0.5f;
        [SerializeField] protected float _pressureStrenght = 8f;
        
        [SerializeField] protected SpriteShapeController _spriteShapeController;
        protected GameObject[] _points;
        protected float _targetArea;
        
        private void Awake()
        {
            CreateVertices();
            _targetArea = CalculateArea();
            MatchPointsOfSpriteShape();
            UpdateSpline();
        }
    
        protected float CalculateArea()
        {
            float area = 0f;
    
            for (int i = 0; i < _points.Length; ++i)
            {
                Vector2 A = _points[i].transform.position;
                Vector2 B  = _points[(i + 1) % _points.Length].transform.position;
    
                area += (A.x * B.y) - (B.x * A.y);
            }
    
            return area;
        }
    
        protected void ApplyPressure()
        {
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
    
        private void CreateVertices()
        {
            _points = new GameObject[_numberOfVertices];
    
            float interval = 2f*MathF.PI / (_numberOfVertices);
            float t = 0f;
    
            for (int i = 0; i < _numberOfVertices; ++i)
            {
                _points[i] = new GameObject($"Point {i}", new Type[]{typeof(CircleCollider2D), typeof(Rigidbody2D), 
                    typeof(SpringJoint2D), typeof(SpringJoint2D), typeof(DistanceJoint2D), typeof(DistanceJoint2D)});
    
                _points[i].gameObject.tag = "Player";
    
                _points[i].GetComponent<Rigidbody2D>().mass = 0.1f;
                _points[i].GetComponent<Rigidbody2D>().freezeRotation = true;
                _points[i].GetComponent<Rigidbody2D>().angularDamping = 0.0f;
                _points[i].GetComponent<CircleCollider2D>().radius = _pointRadius;
                _points[i].transform.SetParent(this.transform);
                _points[i].transform.localPosition = new Vector3(MathF.Cos(t), MathF.Sin(t), 0f).normalized * _radius;
                
                t += interval;
            }
    
            for (int i = 0; i < _numberOfVertices; ++i)
            {
                GameObject prev = _points[(i - 1 + _numberOfVertices) % _numberOfVertices];
                GameObject next = _points[(i + 1) % _numberOfVertices];
                SpringJoint2D[] Springs = _points[i].GetComponents<SpringJoint2D>();
                Springs[0].connectedBody = prev.GetComponent<Rigidbody2D>();
                Springs[1].connectedBody = next.GetComponent<Rigidbody2D>();
    
                GameObject prev2 = _points[(i - 2 + _numberOfVertices) % _numberOfVertices];
                GameObject next2 = _points[(i + 2) % _numberOfVertices];
                DistanceJoint2D[] DistanceJoints = _points[i].GetComponents<DistanceJoint2D>();
                DistanceJoints[0].connectedBody = prev2.GetComponent<Rigidbody2D>();
                DistanceJoints[1].connectedBody = next2.GetComponent<Rigidbody2D>();
                
                float[] distances = new float[4];
                distances[0] = Vector2.Distance(_points[i].transform.position, prev.transform.position);
                distances[1] = Vector2.Distance(_points[i].transform.position, next.transform.position);
                distances[2] = Vector2.Distance(_points[i].transform.position, prev2.transform.position);
                distances[3] = Vector2.Distance(_points[i].transform.position, next2.transform.position);
                for (int j = 0; j < 2; ++j)
                {
                    Springs[j].distance = distances[j];
                    Springs[j].frequency = 5f;
                    DistanceJoints[j].distance = distances[j + 2];
                }
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
        }
    
        private void UpdateSpline()
        {
            Vector2 center = GetCenter();
            _anchor.position = center;
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
                
                
                
                Vector2 prev = _points[(i - 1 +  _points.Length) % _points.Length].transform.localPosition;
                Vector2 prevToCenter = (center - prev).normalized;
                Vector2 prevEdge = prev - prevToCenter * _points[(i - 1 +  _points.Length) % _points.Length].GetComponent<CircleCollider2D>().radius;
                
                Vector2 next = _points[(i + 1) % _points.Length].transform.localPosition;
                Vector2 nextToCenter = (center - next).normalized;
                Vector2 nextEdge = next - nextToCenter * _points[(i + 1) % _points.Length].GetComponent<CircleCollider2D>().radius;
                
                Vector2 rightTangent = (nextEdge - prevEdge).normalized;
                
                float lenght = (prev - vertex).magnitude + (next - vertex).magnitude;
                rightTangent *= lenght * _tangentFactor;
                
                _spriteShapeController.spline.SetLeftTangent(i, -rightTangent);
                _spriteShapeController.spline.SetRightTangent(i, rightTangent);
            }
        }
        
        private Vector2 GetCenter()
        {
            Vector2 sum = Vector2.zero;
    
            for (int i = 0; i < _points.Length; ++i)
            {
                sum += (Vector2)_points[i].transform.position;
            }
            
            return sum / _points.Length;
        }
    }
}

