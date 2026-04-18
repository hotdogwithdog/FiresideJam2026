using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class SoftBody : MonoBehaviour
{
    [SerializeField] private float _offset = 0.5f;
    
    [SerializeField] private SpriteShapeController _spriteShapeController;
    [SerializeField] private Transform[] _points;

    private void Awake()
    {
        UpdateVertices();
    }

    private void Update()
    {
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < _points.Length; ++i)
        {
            Vector2 vertex = _points[i].localPosition;

            Vector2 pointToCenter = -vertex.normalized;
            
            float colliderRadius = _points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            try
            {
                _spriteShapeController.spline.SetPosition(i, vertex - pointToCenter * colliderRadius);
            }
            catch
            {
                Debug.Log("Spline points are too close to each other.. recalculate them");
                _spriteShapeController.spline.SetPosition(i, vertex - pointToCenter * (colliderRadius + _offset));
            }

            Vector2 leftTangent = _spriteShapeController.spline.GetLeftTangent(i);
            
            Vector2 newRightTangent = Vector2.Perpendicular(pointToCenter) * leftTangent.magnitude;
            
            _spriteShapeController.spline.SetLeftTangent(i, -newRightTangent);
            _spriteShapeController.spline.SetRightTangent(i, newRightTangent);
        }
    }
}
