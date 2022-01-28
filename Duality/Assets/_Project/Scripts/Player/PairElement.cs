using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
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

        [Header("Settings")] 
        public PairElementMode mode;
        public Color pairColor;
        public float damagePercentile = 0.15f;

        private void Start()
        {
            hollowSprite.color = pairColor;
            filledSprite.color = pairColor;
            ResolveTrails();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (mode == PairElementMode.Spin && col.gameObject.TryGetComponent(out IDamageable damagable))
            {
                damagable.TakeDamage(damagePercentile);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            print(LayerMask.LayerToName(other.gameObject.layer));
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

        public void ChangeMode(PairElementMode newMode)
        {
            mode = newMode;
            ResolveTrails();
        }
    }
}
