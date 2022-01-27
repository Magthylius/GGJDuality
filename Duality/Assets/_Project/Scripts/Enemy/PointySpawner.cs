using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
using Duality.Player;
using Magthylius;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Duality.Enemy
{
    public class PointySpawner : MonoBehaviourPooler<EnemyBase>
    {
        [Header("References")]
        public PlayerController player;
        public MMFeedbacks deathFeedbacks;
        
        [Header("Settings")]
        public float minRange;
        public float maxRange;
        public float spawnInterval;
        public int maxSpawns = 10;

        protected override void AwakeInitialization()
        {
            player.InitializedEvent += Cor;
                
            void Cor() => StartCoroutine(Spawn());;   
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (ActiveCount < maxSpawns)
                {
                    var enemy = Scoop();
                    enemy.transform.position = (Vector3)RandomEx.PointInCircle(minRange, maxRange) + player.MainTR.position;
                    enemy.StartAllCoroutines();
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        protected override void OnPoolableFilled(EnemyBase poolable)
        {
            poolable.deathFeedback = deathFeedbacks;
        }
    }

}