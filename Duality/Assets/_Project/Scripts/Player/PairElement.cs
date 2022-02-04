using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
using Duality.Enemy;
using UnityEngine;
using Magthylius;
using MoreMountains.Feedbacks;

namespace Duality.Player
{
    public class PairElement : MonoBehaviour
    {
        [Header("References")] 
        public PlayerController controller;
        public MMFeedbacks deathFeedback;
        public new Rigidbody2D rigidbody;
        public TrailRenderer spinTrail;
        public TrailRenderer moveTrail;
        public SpriteRenderer hollowSprite;
        public SpriteRenderer filledSprite;
        public GameObject hollowElement;
        public GameObject filledElement;
        public LayerMask damagerLayers;
        public List<Collider2D> relevantColliders;

        [Header("Settings")] 
        public PairElementMode mode;
        public Color pairColor;
        public float damagePercentile = 0.15f;

        private Color _originalSpinColor;

        private void Start()
        {
            hollowSprite.color = pairColor;
            filledSprite.color = pairColor;
            ResolveTrails();

            _originalSpinColor = hollowSprite.color;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (mode == PairElementMode.Spin && col.gameObject.TryGetComponent(out IDamageable damagable))
            {
                damagable.TakeDamage(damagePercentile);
                if (controller.ExplosiveCharged)
                {
                    Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, controller.explosionRadius);
                    foreach (var o in objects)
                    {
                        if (relevantColliders.Contains(o)) continue;

                        float nDistance = o.GetComponent<Rigidbody2D>().AddExplosionForce
                            (transform.position, controller.explosionRadius, controller.explosionForce, ForceMode2D.Impulse);

                        if (o.TryGetComponent(out EnemyBase enemy)) enemy.ExplosionDamage(damagePercentile * nDistance);
                        
                    }

                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (controller.godMode) return;
            if (damagerLayers.Contains(other))
            {
                controller.Kill();
            }
        }

        public void OnDeath()
        {
            spinTrail.emitting = false;
            moveTrail.emitting = false;
            filledElement.SetActive(false);
            hollowElement.SetActive(false);
            deathFeedback.PlayFeedbacks(transform.position);

            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0f;
            rigidbody.rotation = 0f;
        }

        public void OnRespawn()
        {
            ResolveTrails();
        }

        private void ResolveTrails()
        {
            switch (mode)
            {
                case PairElementMode.Move:
                    spinTrail.emitting = false;
                    moveTrail.emitting = true;
                    filledElement.SetActive(true);
                    hollowElement.SetActive(false);
                    break;
                
                case PairElementMode.Spin:
                    spinTrail.emitting = true;
                    moveTrail.emitting = false;
                    filledElement.SetActive(false);
                    hollowElement.SetActive(true);
                    break;
            }
        }
        
        public void SetHollowColor(Color newColor)
        {
            hollowSprite.color = newColor;
            spinTrail.colorGradient.colorKeys[0].color = newColor;
        }

        public void ChangeMode(PairElementMode newMode)
        {
            mode = newMode;
            ResolveTrails();
        }

        public Color OriginalSpinColor => _originalSpinColor;
    }
}
