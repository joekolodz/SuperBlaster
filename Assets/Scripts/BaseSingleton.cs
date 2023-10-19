using UnityEngine;

namespace Assets.Scripts
{
    public class BaseSingleton<T> : MonoBehaviour where T: BaseSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new GameObject().AddComponent<T>();
                _instance.name = $"*** Singleton: {_instance.GetType().Name} ***";
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }
    }
}
