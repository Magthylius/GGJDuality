using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
using UnityEngine;
using Magthylius;

namespace Duality.Player
{
    public class PairElement : MonoBehaviour
    {
        [Header("References")]
        public new Rigidbody2D rigidbody;
        public TrailRenderer spinTrail;
        public TrailRenderer moveTrail;
        public SpriteRenderer hollowSprite;
        public SpriteRenderer filledSprite;

        [Header("Settings")] 
        public PairElementMode mode;
        public Color pairColor;

        private float _currentFrameImpulseSqr = 0f;
        private float _previousFrameImpulseSqr = 0f;

        private void Start()
        {
            hollowSprite.color = pairColor;
            filledSprite.color = pairColor;
            ResolveTrails();
        }

        private void FixedUpdate()
        {
            _previousFrameImpulseSqr = _currentFrameImpulseSqr;
            _currentFrameImpulseSqr = rigidbody.SpeedSqr();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            print(col.gameObject.name);
            
            if (mode == PairElementMode.Spin && col.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(0.25f);
                //float hit = Mathf.Abs(_currentFrameImpulseSqr - _previousFrameImpulseSqr);
                //( hit);
            }
        }

        private void ResolveTrails()
        {
            switch (mode)
            {
                case PairElementMode.Move:
                    spinTrail.emitting = false;
                    moveTrail.emitting = true;
                    filledSprite.enabled = true;
                    hollowSprite.enabled = false;
                    break;
                
                case PairElementMode.Spin:
                    spinTrail.emitting = true;
                    moveTrail.emitting = false;
                    filledSprite.enabled = false;
                    hollowSprite.enabled = true;
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
