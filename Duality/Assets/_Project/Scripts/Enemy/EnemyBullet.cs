using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
using Magthylius;
using Unity.VisualScripting;
using UnityEngine;

namespace Duality.Enemy
{
    public class EnemyBullet : MonoBehaviourPoolable, IDamager
    {
        public new Rigidbody2D rigidbody;
        public TrailRenderer trail;
        public float lifeTime = 5f;

        
        public void Shoot(Vector3 position, Vector2 force)
        {
            transform.position = position;
            trail.Clear();
            trail.emitting = true;
            transform.rotation = Quaternion.Euler(0f, 0f, MathEx.Atan2Deg(force.y, force.x) + 90f);
            rigidbody.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(LifeDecay());
        }

        private IEnumerator LifeDecay()
        {
            yield return new WaitForSeconds(lifeTime);
            trail.Clear();
            trail.emitting = false;
            Dump();
        }
    }
}
