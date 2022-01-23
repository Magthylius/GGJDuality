using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class ColliderEx
    {
        /// <summary>Disable and reenables the collider to force collision to trigger again.</summary>
        public static void Retrigger(this Collider2D collider)
        {
            collider.enabled = false;
            collider.enabled = true;
        }

        /// <summary>Disable and reenables the collider to force collision to trigger again.</summary>
        public static void Retrigger(this Collider collider)
        {
            collider.enabled = false;
            collider.enabled = true;
        }

        /// <summary>Parent of this collider transform.</summary>
        public static Transform Parent(this Collider2D collider) => collider.transform.parent;

        /// <summary>Parent of this collider transform.</summary>
        public static Transform Parent(this Collider collider) => collider.transform.parent;
    }

    public static class RaycastHitEx
    {
        /// <summary>Parent of this ray cast hit transform.</summary>
        public static Transform Parent(this RaycastHit2D hit) => hit.collider.transform.parent;

        /// <summary>Parent of this ray cast hit transform.</summary>
        public static Transform Parent(this RaycastHit hit) => hit.collider.transform.parent;

        /// <summary>GameObject of this ray cast hit.</summary>
        public static GameObject GameObject(this RaycastHit2D hit) => hit.collider.gameObject;

        /// <summary>GameObject of this ray cast hit.</summary>
        public static GameObject GameObject(this RaycastHit hit) => hit.collider.gameObject;

        /// <summary>Layer of this ray cast hit game object.</summary>
        public static int Layer(this RaycastHit2D hit) => hit.collider.gameObject.layer;

        /// <summary>Layer of this ray cast hit game object.</summary>
        public static int Layer(this RaycastHit hit) => hit.collider.gameObject.layer;
    }

    public static class LayerEx
    {
        /// <summary>Checks if a layer is in a mask.</summary>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}
