using UnityEngine;

namespace Utils.Singletons
{
    public class HiddenSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static bool shuttingDown;
        private static object lockObj = new();
        private static T instance;

        protected static T Instance
        {
            get
            {
                if (!Application.isPlaying) return null;
            
                if (shuttingDown)
                {
                    //Debug.LogWarning("[Instance] Instance '" + typeof(T) +
                    //    "' already destroyed. Returning null.");
                    return null;
                }

                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));
                    }

                    return instance;
                }
            }
        }
        
        private void OnEnable()
        {
            shuttingDown = false;
        }


        private void OnApplicationQuit()
        {
            shuttingDown = true;
        }


        private void OnDestroy()
        {
            shuttingDown = true;
        }
    }
}