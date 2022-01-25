using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public abstract class ObjectPooler<T> : MonoBehaviour where T : IPoolable
    {
        public int initialPoolAmount = 10;
        protected List<T> pool = new List<T>();

        private void Awake()
        {
            pool = new List<T>();
            FillSome(initialPoolAmount);
            AwakeInitialization();
        }

        protected virtual void AwakeInitialization() { }

        /// <summary>Creates, pool and deactivates a poolable element.</summary>
        /// <returns>Newly created element.</returns>
        protected abstract T Fill();

        /// <summary>Creates, pool and deactivates a list of poolable elements.</summary>
        /// <returns>Newly created elements.</returns>
        protected abstract List<T> FillSome(int amount);

        /// <summary>Gets an inactive element from pool, if not, creates one.</summary>
        /// <returns>New active element from pool.</returns>
        public abstract T Scoop();

        /// <summary>Gets an array of inactive elements from pool, if not, create the needed ones.</summary>
        /// <returns>New active  element array from pool.</returns>
        public abstract List<T> ScoopSome(int amount);

        /// <summary>Returns a poolable object into the array.</summary>
        /// <returns>New active  element array from pool.</returns>
        public abstract void Dump(T poolableObject);
    }
}
