using System;
using System.Collections;
using System.Collections.Generic;
using Magthylius;
using UnityEngine;

namespace Duality.Core
{
    public class PlayerFollower : MonoBehaviour
    {
        public Transform follow;
        public Vector3 offset;
        public float lerpSpeed = 5f;

        public void LateUpdate()
        {
            transform.position =
                MathEx.LerpSnap(transform.position, follow.position + offset, lerpSpeed * Time.deltaTime, 0.99f);
        }
    }
}
