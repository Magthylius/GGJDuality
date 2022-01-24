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

        [Header("Settings")] 
        public PairElementMode mode;

        private void Start()
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
                    break;
                
                case PairElementMode.Spin:
                    spinTrail.emitting = true;
                    moveTrail.emitting = false;
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
