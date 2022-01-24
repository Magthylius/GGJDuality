using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {
            hollowSprite.color = pairColor;
            filledSprite.color = pairColor;
            ResolveTrails();
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
