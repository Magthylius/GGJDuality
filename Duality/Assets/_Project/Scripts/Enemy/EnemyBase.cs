using System;
using System.Collections;
using System.Collections.Generic;
using Magthylius;
using UnityEngine;
using Duality.Core;

namespace Duality.Enemy
{
    public class EnemyBase : MonoBehaviourPoolable, IDamagable
    {
        public new Rigidbody2D rigidbody;
        
        public float ragdollTime;
        public bool isRagdolling;
        private float _ragdollStartTime;

        public float deathSpeed = 50f;
        public bool isDead = false;
        
        public Action DamagedEvent { get; set; }
        public Action DeathEvent { get; set; }

        private void Start()
        {
            DeathEvent += OnDeath;
        }

        public void TakeDamage(float damage)
        {
            rigidbody.mass *= 1f - damage;

            isRagdolling = true;
            _ragdollStartTime = Time.timeSinceLevelLoad;
            StartCoroutine(RagdollLogic());
            
            DamagedEvent?.Invoke();
        }

        public void OnDeath()
        {
            //! TODO: Fireworks death
            Dump();
        }

        private IEnumerator RagdollLogic()
        {
            while (true)
            {
                if (rigidbody.SpeedSqr() >= MathEx.Square(deathSpeed))
                {
                    isDead = true;
                    break;
                }
                else if (Time.timeSinceLevelLoad - _ragdollStartTime > ragdollTime)
                {
                    isDead = false;
                    break;
                }

                yield return null;
            }
            
            if (isDead) OnDeath();
            // else AI LOGIC
        }
    }

}