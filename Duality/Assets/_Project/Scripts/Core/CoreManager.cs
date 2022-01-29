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
        public MMFeedbacks titleEndLoadFeedback;
        public MMFeedbacks titleStartLoadFeedback;
    
        [Header("Settings")]
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        private GameState _gameState;
        private int _enemyDeathCount = 0;

        public Action<int> EnemyDeathEvent;
        public Action PlayerDeathEvent;

        public Action<GameState> GameStateChangedEvent;

        private void OnValidate()
        {
            mainCamera.backgroundColor = secondColor;
        }
        
        void Start()
        {
            _gameState = GameState.WaitForStart;
        }
        
        public void ReportEnemyDeath()
        {
            _enemyDeathCount++;
            EnemyDeathEvent?.Invoke(_enemyDeathCount);
        }

        public void ReportPlayerDeath()
        {
            PlayerDeathEvent?.Invoke();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (_gameState)
                {
                    case GameState.WaitForStart:
                    {
                        if (!titleStartLoadFeedback.IsPlaying)
                        {
                            titleEndLoadFeedback.PlayFeedbacks();
                            _gameState = GameState.Gameplay;
                            GameStateChangedEvent?.Invoke(_gameState);
                        }

                        break;
                    }
                }
            }
        }

        public int EnemyDeathCount => _enemyDeathCount;
        public static Color FirstColor => Instance.firstColor;
        public static Color SecondColor => Instance.secondColor;
        public static GameState CurrentState => Instance._gameState;
        public static Transform MainTransform => Instance.player.MainTR;

        public enum GameState
        {
            WaitForStart,
            Gameplay,
            GameEnd
        }
    }
}
