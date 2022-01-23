using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    /// <summary>Base class for soft singleton auto initialization.</summary>
    /// <remarks>Destroys the current script if other Instance is found.</remarks>
    public class SoftSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        private void Awake() 
        {
            if (Instance == null) Instance = this as T;
            else Destroy(this);
        }
    }

    /// <summary>Base class for hard singleton auto initialization.</summary>
    /// <remarks>Destroys the whole GameObject if other Instance is found.</remarks> 
    public class HardSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this as T;
            else Destroy(gameObject);
        }
    }
    
    /// <summary>Base class for soft singleton auto initialization.</summary>
    /// <remarks>Destroys the current script if other Instance is found.</remarks>
    public class SoftSingletonPersistent<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        private void Awake() 
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else Destroy(this);
        }
    }
    
    /// <summary>Base class for hard singleton auto initialization.</summary>
    /// <remarks>Destroys the whole GameObject if other Instance is found.</remarks> 
    public class HardSingletonPersistent<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else Destroy(gameObject);
        }
    }
}
