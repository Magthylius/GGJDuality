using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    /// <summary>Base class for soft singleton auto initialization</summary>
    public class SoftSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        private void Awake() 
        {
            if (Instance == null) Instance = this as T;
            else Destroy(this);
        }
    }

    /// <summary>Base class for hard singleton auto initialization</summary>
    public class HardSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this as T;
            else Destroy(this.gameObject);
        }
    }
}
