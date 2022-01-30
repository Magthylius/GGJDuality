using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Core;
using Duality.Player;
using Magthylius;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

namespace Duality.Enemy
{
    public class PointySpawner : MonoBehaviourPooler<EnemyBase>
    {
        [Header("References")]
        public PlayerController player;
        public MMFeedbacks deathFeedbacks;

        [Header("Settings")]
        public float spawnDelay = 2f;
        public float minRange;
        public float maxRange;
        public float spawnInterval;
        public int maxSpawns = 10;

        private bool _playerInited;
        private bool _gameStarted;
        private bool _playerDead;
        private Coroutine _spawnCor;

        protected override void AwakeInitialization()
        {
            player.InitializedEvent += PlayerInit;
            CoreManager.GameStartedEvent += GameStarted;
            CoreManager.GameEndedEvent += EndGame;
            CoreManager.PlayerSpawnEvent += OnPlayerSpawn;
            CoreManager.PlayerDeathEvent += OnPlayerDeath;
        }
        
        void PlayerInit()
        {
            _playerInited = true;
            StartGame();
        }

        void GameStarted()
        {
            _gameStarted = true;
            StartGame();
        }

        private void StartGame()
        {
            if (_playerInited && _gameStarted)
                _spawnCor = StartCoroutine(Spawn());
        }

        private void EndGame()
        {
            if (_spawnCor != null) StopCoroutine(_spawnCor);
            StopAllCoroutines();
        }

        private void OnPlayerDeath()
        {
            _playerDead = true;
        }

        private void OnPlayerSpawn()
        {
            _playerDead = false;
            DumpAll();
            _spawnCor = StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            if (_playerDead) yield break;
            yield return new WaitForSeconds(spawnDelay);

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
            CoreManager.PlayerDeathEvent += poolable.OnPlayerDeath;
            CoreManager.PlayerSpawnEvent += poolable.OnPlayerSpawn;
        }
    }

}