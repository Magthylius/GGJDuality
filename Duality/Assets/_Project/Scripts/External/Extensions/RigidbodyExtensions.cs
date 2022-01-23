using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class RigidbodyEx
    {
        public static float SpeedSqr(this Rigidbody rb) => rb.velocity.sqrMagnitude;
        public static float Speed(this Rigidbody rb) => rb.velocity.magnitude;
    }
}