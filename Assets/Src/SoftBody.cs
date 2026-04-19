using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class SoftBody : MonoBehaviour
{
    [SerializeField] private int _numberOfVertices = 16;
    [SerializeField] private float _radius = 2f;
    
    [SerializeField] private SpriteShapeController _spriteShapeController;
    private GameObject[] _points;

    private void Awake()
    {
        CreateVertices();
        MatchPointsOfSpriteShape();
        UpdateVertices();
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

            _points[i].GetComponent<Rigidbody2D>().mass = 0.1f;
            _points[i].GetComponent<Rigidbody2D>().freezeRotation = true;
            _points[i].GetComponent<CircleCollider2D>().radius = 0.25f;
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

    private void Update()
    {
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        Vector2 center = GetCenter();
        for (int i = 0; i < _points.Length; ++i)
        {
            Vector2 vertex = _points[i].transform.localPosition;

            Vector2 pointToCenter = (center - vertex).normalized;
            
            float colliderRadius = _points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            _spriteShapeController.spline.SetPosition(i, vertex - pointToCenter * colliderRadius);

            //Vector2 leftTangent = _spriteShapeController.spline.GetLeftTangent(i);
            
            //Vector2 newRightTangent = Vector2.Perpendicular(pointToCenter) * leftTangent.magnitude;
            
            Vector2 prev = _points[(i - 1 +  _points.Length) % _points.Length].transform.localPosition;
            Vector2 prevToCenter = (center - prev).normalized;
            Vector2 prevEdge = prev + prevToCenter * _points[(i - 1 +  _points.Length) % _points.Length].GetComponent<CircleCollider2D>().radius;
            
            Vector2 next = _points[(i + 1) % _points.Length].transform.localPosition;
            Vector2 nextToCenter = (center - next).normalized;
            Vector2 nextEdge = next + nextToCenter * _points[(i + 1) % _points.Length].GetComponent<CircleCollider2D>().radius;
            
            Vector2 rightTangent = (nextEdge - prevEdge).normalized * 0.5f;
            
            _spriteShapeController.spline.SetLeftTangent(i, -rightTangent);
            _spriteShapeController.spline.SetRightTangent(i, rightTangent);
        }
    }
    
    private Vector2 GetCenter()
    {
        Vector2 sum = Vector2.zero;

        for (int i = 0; i < _points.Length; ++i)
        {
            sum += (Vector2)_points[i].transform.localPosition;
        }
        
        return sum / _points.Length;
    }
}
