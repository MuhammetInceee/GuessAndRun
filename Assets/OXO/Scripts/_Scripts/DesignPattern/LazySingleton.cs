using UnityEngine;

namespace MuhammetInce.DesignPattern.Singleton
{
    public class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance == null) Instance = GameObject.FindObjectOfType<T>();
            else Destroy(gameObject);
        }
    }
}
