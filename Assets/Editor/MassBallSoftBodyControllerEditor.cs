#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(SoftBodyControllers.MassBallSoftBodyController))]
    public class MassBallSoftBodyControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SoftBodyControllers.MassBallSoftBodyController sb = (SoftBodyControllers.MassBallSoftBodyController)target;

            if (GUILayout.Button("Bake Nodes"))
            {
                sb.BakeNodes();
            }
        }
    }
}
#endif