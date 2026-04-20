using System;
using UnityEngine;

namespace SoftBodyControllers
{
    public class SlimeSoftBodyController : SoftBody
    {
        [Header("Generation Configuration")]
        [SerializeField] private int _numberOfVertices = 16;
        [SerializeField] private float _radius = 2f;
        [SerializeField] private float _pointRadius = 0.25f;
        [SerializeField] private float _pointMass = 0.1f;
        [SerializeField] private float _springFrequency = 5f;

        // The points are used the same to not duplicate because are more expensive that just 2 floats and just with the lenght it must be okey
        [HideInInspector] [SerializeField] private float _bakedRadius;
        [HideInInspector] [SerializeField] private float _bakedPointRadius;
        [HideInInspector] [SerializeField] private float _bakedPointMass;
        [HideInInspector] [SerializeField] private float _bakedSpringFrequency;
        [HideInInspector] [SerializeField] private float _bakedScale;
        
        protected override void CreatePoints()
        {
            if (IsBakingValid())
            {
                Debug.Log("SlimeSoftBodyController::CreatePoints: Using the baked points");
                return;
            }
            Debug.Log("SlimeSoftBodyController::CreatePoints: Generating points");
            BakeValues();
            
            // Actual Start of the generation xd
            _points = new GameObject[_numberOfVertices];
    
            float interval = 2f*MathF.PI / (_numberOfVertices);
            float t = 0f;
    
            for (int i = 0; i < _numberOfVertices; ++i)
            {
                _points[i] = new GameObject($"Point {i}", new Type[]{typeof(CircleCollider2D), typeof(Rigidbody2D), 
                    typeof(SpringJoint2D), typeof(SpringJoint2D), typeof(DistanceJoint2D), typeof(DistanceJoint2D)});
    
                _points[i].gameObject.tag = "Player";
                
                Rigidbody2D pointRb = _points[i].GetComponent<Rigidbody2D>();
                pointRb.mass = _pointMass;
                pointRb.freezeRotation = true;
                pointRb.angularDamping = 0.0f;
                
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
                Springs[0].autoConfigureDistance = false;
                Springs[1].autoConfigureDistance = false;
                
    
                GameObject prev2 = _points[(i - 2 + _numberOfVertices) % _numberOfVertices];
                GameObject next2 = _points[(i + 2) % _numberOfVertices];
                DistanceJoint2D[] DistanceJoints = _points[i].GetComponents<DistanceJoint2D>();
                DistanceJoints[0].connectedBody = prev2.GetComponent<Rigidbody2D>();
                DistanceJoints[1].connectedBody = next2.GetComponent<Rigidbody2D>();
                DistanceJoints[0].autoConfigureDistance = false;
                DistanceJoints[1].autoConfigureDistance = false;
                
                float[] distances = new float[4];
                distances[0] = Vector2.Distance(_points[i].transform.position, prev.transform.position);
                distances[1] = Vector2.Distance(_points[i].transform.position, next.transform.position);
                distances[2] = Vector2.Distance(_points[i].transform.position, prev2.transform.position);
                distances[3] = Vector2.Distance(_points[i].transform.position, next2.transform.position);
                
                // Apply distances to joints
                for (int j = 0; j < 2; ++j)
                {
                    Springs[j].distance = distances[j];
                    Springs[j].frequency = _springFrequency;
                    DistanceJoints[j].distance = distances[j + 2];
                }
            }
        }
        
        
        #region Baking of nodes and clear of points
        private void BakeValues()
        {
            ClearPointsObjects();
            
            _bakedRadius = _radius;
            _bakedPointRadius = _pointRadius;
            _bakedPointMass = _pointMass;
            _bakedSpringFrequency = _springFrequency;
            _bakedScale = _scale;
        }

        private bool IsBakingValid()
        {
            if (_points != null && _points.Length == _numberOfVertices && _bakedRadius == _radius &&
                _bakedPointRadius == _pointRadius &&
                _bakedPointMass == _pointMass && _bakedSpringFrequency == _springFrequency &&
                _bakedScale == _scale) // Metadata
            {
                // Actual points
                for (int i = 0; i < _points.Length; ++i)
                {
                    if (_points[i] == null) return false;
                }

                return true;
            }
            return false;
        }

        private void ClearPointsObjects()
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
        
        // Used by the editor to have the button
        public void BakeNodes()
        {
            ClearPointsObjects();
            CreatePoints();
        }
        #endregion
    }
}