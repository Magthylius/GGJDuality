using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public interface IPoolable
    {
        public bool IsActive();
        public void Scoop();
        public void Dump();
    }

    public interface ICollidable2D
    {
        public System.Action<Collision2D> CollisionEnteredEvent { get; set; }
        public System.Action<Collision2D> CollisionExitedEvent { get; set; }
    }

    public interface ITriggerable2D
    {
        public System.Action<Collider2D> TriggerEnteredEvent { get; set; }
        public System.Action<Collider2D> TriggerExitedEvent { get; set; }
    }

    public interface IInitializable
    {
        public bool isInitialized { get; set; }
        public System.Action InitializedEvent { get; set; }
    }
}
