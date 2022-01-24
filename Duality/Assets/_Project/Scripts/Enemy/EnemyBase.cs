using System;
using System.Collections;
using System.Collections.Generic;
using Magthylius;
using UnityEngine;
using Duality.Core;
using MoreMountains.Feedbacks;

namespace Duality.Enemy
{
    public class EnemyBase : MonoBehaviourPoolable, IDamagable
    {
        [Header("References")]
        public new Rigidbody2D rigidbody;
        public MMFeedbacks deathFeedback;
        
        [Header("Ragdoll Settings")]
        public float ragdollTime;
        public bool isRagdolling;
        private float _ragdollStartTime;
        
        [Header("Death Settings")]
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
            //(MMFeedbackParticlesInstantiation)deathFeedback.Feedbacks[0].
            deathFeedback.PlayFeedbacks(transform.position);
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