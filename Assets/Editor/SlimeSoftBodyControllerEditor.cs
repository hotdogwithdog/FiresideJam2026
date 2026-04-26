#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(SoftBodyControllers.SlimeSoftBodyController))]
    public class SlimeSoftBodyControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SoftBodyControllers.SlimeSoftBodyController sb = (SoftBodyControllers.SlimeSoftBodyController)target;

            if (GUILayout.Button("Bake Nodes"))
            {
                sb.BakeNodes();
            }
        }
    }
}
#endif