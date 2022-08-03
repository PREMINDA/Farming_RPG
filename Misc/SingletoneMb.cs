using UnityEngine;

namespace Script.Misc
{
    public abstract class SingletoneMb<T> : MonoBehaviour where T:MonoBehaviour
    {
        public static T Instance { get; set;}

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

