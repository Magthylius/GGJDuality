using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class TransformEx
    {
        /// <summary>Destroys all children in a Transform object.</summary>
        /// <param name="parent">Transform of GameObject in which children will be claered.</param>
        /// <returns>Number of children destroyed.</returns>
        public static int DestroyChildren(Transform parent)
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child == parent) continue;
                Object.Destroy(child.gameObject);
            }

            return children.Length - 1;
        }

        /// <summary>Destroys all children in a Transform object.</summary>
        /// <param name="parent">Transform of GameObject in which children will be claered.</param>
        /// <returns>Number of children destroyed.</returns>
        public static int DestroyChildren(GameObject parent)
        {
            return DestroyChildren(parent.transform);
        }

        /// <summary>Destroys all children in a Transform object, immediately.</summary>
        /// <param name="parent">Transform of GameObject in which children will be claered.</param>
        /// <returns>Number of children destroyed.</returns>
        public static int DestroyChildrenImmediate(Transform parent)
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child == parent) continue;
                Object.DestroyImmediate(child.gameObject);
            }

            return children.Length - 1;
        }

        /// <summary>Destroys all children in a Transform object, immediately.</summary>
        /// <param name="parent">Transform of GameObject in which children will be claered.</param>
        /// <returns>Number of children destroyed.</returns>
        public static int DestroyChildrenImmediate(GameObject parent)
        {
            return DestroyChildren(parent.transform);
        }

    }
}
