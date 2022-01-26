using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
using Magthylius;
using UnityEngine;

namespace Duality.Enemy
{
    public class PointySpawner : MonoBehaviourPooler<EnemyBase>
    {
        public float minRange;
        public float maxRange;
        public float spawnInterval;
        public int maxSpawns = 10;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        public void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Transform main = CoreManager.MainTransform;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(main.position, minRange);
            Gizmos.DrawWireSphere(main.position, maxRange);
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (ActiveCount < maxSpawns)
                {
                    var enemy = Scoop();
                    enemy.transform.position = RandomEx.PointInCircle(minRange, maxRange);
                    enemy.StartAllCoroutines();
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

}