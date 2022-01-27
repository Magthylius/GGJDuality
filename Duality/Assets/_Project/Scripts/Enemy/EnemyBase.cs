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
        public float initialMass;
        public float ragdollTime;
        
        private float _ragdollStartTime;
        private bool _isRagdolling;
        
        [Header("Death Settings")]
        public float deathSpeed = 50f;
        public bool isDead = false;
        
        [Header("Tracking settings")]
        public float trackInterval = 2f;
        public float trackStopDistance = 5f;
        public float moveSpeed = 5f;
        public float stopSpeed = 5f;
        public float rotSpeed = 5f;
        public float rotOffset = 90f;
        
        private Vector3 _targetPosition;
        private Vector3 _targetDirection;

        public Action DamagedEvent { get; set; }
        public Action DeathEvent { get; set; }

        private void Start()
        {
            DeathEvent += OnDeath;
            StartAllCoroutines();
            ResetRigidbody();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, trackStopDistance);
        }

        private void FixedUpdate()
        {
            if (!_isRagdolling)
            {
                if (!MathEx.InRange(_targetPosition - transform.position, trackStopDistance))
                    rigidbody.velocity = _targetDirection * moveSpeed * Time.deltaTime;
                else
                    rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Vector2.zero, stopSpeed * Time.deltaTime);
                
                Quaternion targetRot = Quaternion.Euler(0f, 0f, MathEx.Atan2Deg(_targetDirection.y, _targetDirection.x) + rotOffset);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
                rigidbody.angularVelocity = Mathf.Lerp(rigidbody.angularVelocity, 0f, rotSpeed * Time.deltaTime);
            }
        }

        public void StartAllCoroutines()
        {
            StartCoroutine(TrackTarget());
            StartCoroutine(AILogic());
        }

        public void TakeDamage(float damage)
        {
            rigidbody.mass *= 1f - damage;

            _isRagdolling = true;
            _ragdollStartTime = Time.timeSinceLevelLoad;

            DamagedEvent?.Invoke();
        }

        public void OnDeath()
        {
            Dump();
            ResetRigidbody();
            StopCoroutine(nameof(AILogic));
            deathFeedback.PlayFeedbacks(transform.position);
        }

        private void ResetRigidbody()
        {
            rigidbody.mass = initialMass;
            rigidbody.angularVelocity = 0f;
            rigidbody.velocity = Vector2.zero;
        }

        private IEnumerator AILogic()
        {
            while (true)
            {
                if (_isRagdolling)
                {
                    if (rigidbody.SpeedSqr() >= MathEx.Square(deathSpeed))
                    {
                        isDead = true;
                        OnDeath();
                        break;
                    }
                    
                    if (Time.timeSinceLevelLoad - _ragdollStartTime > ragdollTime)
                    {
                        isDead = false;
                        _isRagdolling = false;
                        rigidbody.angularVelocity = 0f;
                    }
                }
                else
                {
                    _targetDirection = (_targetPosition - transform.position).normalized;
                }

                yield return null;
            }
        }
        
        private IEnumerator TrackTarget()
        {
            while (true)
            {
                _targetPosition = CoreManager.MainTransform.position;
                yield return new WaitForSeconds(trackInterval);
            }
        }
    }

}