using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Player;
using UnityEngine;
using Magthylius;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;

namespace Duality.Core
{
    public class CoreManager : SoftSingletonPersistent<CoreManager>
    {
        [Header("References")] 
        public Camera mainCamera;
        public PlayerController player;
        public Transform playerSpawnLocation;
        public MMFeedbacks titleEndLoadFeedback;
        public MMFeedbacks titleStartLoadFeedback;
    
        [Header("Settings")]
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        private GameState _currentGameState;
        private int _enemyDeathCount = 0;

        public Action<int> EnemyDeathEvent;
        public static Action PlayerDeathEvent;
        public static Action PlayerSpawnEvent;

        public static Action<GameState> GameStateChangedEvent;
        public static Action GameStartedEvent;
        public static Action GameEndedEvent;

        private void OnValidate()
        {
            mainCamera.backgroundColor = secondColor;
        }
        
        void Start()
        {
            CurrentGameState = GameState.WaitForStart;
        }
        
        public void ReportEnemyDeath()
        {
            _enemyDeathCount++;
            EnemyDeathEvent?.Invoke(_enemyDeathCount);
        }

        public void ReportPlayerDeath()
        {
            PlayerDeathEvent?.Invoke();
            CurrentGameState = GameState.Ended;
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (_currentGameState)
                {
                    case GameState.WaitForStart:
                    {
                        if (!titleStartLoadFeedback.IsPlaying)
                        {
                            titleEndLoadFeedback.PlayFeedbacks();
                            CurrentGameState = GameState.Starting;
                        }

                        break;
                    }

                    case GameState.Starting:
                    {
                        if (!titleEndLoadFeedback.IsPlaying)
                        {
                            CurrentGameState = GameState.Started;
                        }
                        break;
                    }
                }
            }
        }

        public void Restart()
        {
            CurrentGameState = GameState.Starting;
            player.MainTR.position = playerSpawnLocation.position;

            _enemyDeathCount = 0;
        }

        public GameState CurrentGameState
        {
            get => _currentGameState;
            private set
            {
                _currentGameState = value;
                GameStateChangedEvent?.Invoke(value);
                switch (value)
                {
                   case GameState.Started:
                       GameStartedEvent?.Invoke();
                       break;
                   
                   case GameState.Ended:
                       GameEndedEvent?.Invoke();
                       break;
                }
            }
        }

        public int EnemyDeathCount => _enemyDeathCount;
        public static Color FirstColor => Instance.firstColor;
        public static Color SecondColor => Instance.secondColor;
        public static GameState CurrentState => Instance._currentGameState;
        public static Transform MainTransform => Instance.player.MainTR;

        public enum GameState
        {
            WaitForStart,
            Starting,
            Started,
            Ended
        }
    }
}
