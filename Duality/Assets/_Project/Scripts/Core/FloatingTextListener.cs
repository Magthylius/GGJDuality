using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace Duality.Core
{
    public class FloatingTextListener : MonoBehaviour
    {
        public MMFloatingText floatingText;
        private CoreManager core;

        private void Awake()
        {
            core = CoreManager.Instance;
            core.EnemyDeathEvent += OnEnemyDeath;
        }

        private void OnEnemyDeath(int currentKillCount)
        {
            floatingText.SetText(currentKillCount.ToString());
        }
    }
}
