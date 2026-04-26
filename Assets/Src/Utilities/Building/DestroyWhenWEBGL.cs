namespace Utilities.Building
{
    public class DestroyWhenWEBGL : UnityEngine.MonoBehaviour
    {
        private void Awake()
        {
            #if UNITY_WEBGL
                Destroy(this.gameObject); 
            #endif
        }
    }
}