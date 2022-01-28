using System;
using System.Collections;
using System.Collections.Generic;
using Magthylius;
using UnityEngine;
using Duality.Core;
using MoreMountains.Feedbacks;

namespace Duality.Enemy
{
    public class EnemyBase : MonoBehaviourPoolable, IDamageable
    {
        [Header("References")]
        public new Rigidbody2D rigidbody;
        public MMFeedbacks deathFeedback;
        public LineRenderer line;
        public Transform shootPoint;

        private CoreManager core;

        [Header("Ragdoll Settings")] 
        public float initialMass;
        public float ragdollTime;
        
        private float _ragdollStartTime;

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

        [Header("Aim settings")] 
        public float aimTime;
        public float shootImpulseForce = 10f;

        private EnemyMode mode = EnemyMode.Normal;

        public Action DamagedEvent { get; set; }
        public Action DeathEvent { get; set; }

        private void Start()
        {
            DeathEvent += OnDeath;
            StartAllCoroutines();
            ResetRigidbody();
            DisableLine();
            mode = EnemyMode.Normal;

            core = CoreManager.Instance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, trackStopDistance);
        }

        private void FixedUpdate()
        {
            if (mode != EnemyMode.Ragdoll)
            {
                if (mode != EnemyMode.Aiming && !MathEx.InRange(_targetPosition - transform.position, trackStopDistance))
                    rigidbody.velocity = _targetDirection * moveSpeed * Time.deltaTime;
                else
                {
                    rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Vector2.zero, stopSpeed * Time.deltaTime);
                    if (mode != EnemyMode.Aiming) StartAim();
                }
                
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

            mode = EnemyMode.Ragdoll;
            _ragdollStartTime = Time.timeSinceLevelLoad;
            DisableLine();
            StopCoroutine(nameof(AimUpdate));
            DamagedEvent?.Invoke();
        }

        public void OnDeath()
        {
            Dump();
            ResetRigidbody();
            DisableLine();
            mode = EnemyMode.Normal;
            StopCoroutine(nameof(AILogic));
            deathFeedback.PlayFeedbacks(transform.position);
            core.ReportEnemyDeath();
        }

        private void ResetRigidbody()
        {
            rigidbody.mass = initialMass;
            rigidbody.angularVelocity = 0f;
            rigidbody.velocity = Vector2.zero;
        }

        private void StartAim()
        {
            mode = EnemyMode.Aiming;
            EnableLine();
            StartCoroutine(nameof(AimUpdate));
        }

        private void DisableLine()
        {
            line.enabled = false;
        }

        private void EnableLine()
        {
            line.enabled = true;
        }

        private IEnumerator AILogic()
        {
            while (true)
            {
                if (mode == EnemyMode.Ragdoll)
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
                        mode = EnemyMode.Normal;
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

        private IEnumerator AimUpdate()
        {
            yield return new WaitForSeconds(aimTime);
            EnemyBulletPooler.Instance.Scoop().Shoot(shootPoint.transform.position, transform.up * shootImpulseForce);
        }
    }

}