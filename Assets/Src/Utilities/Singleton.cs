using UnityEngine;


namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }
        
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                UnityEngine.Debug.LogError($"The Singleton of type: {typeof(T)}, already exists, Destroying the new object");
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(Instance);
        }
    }
}