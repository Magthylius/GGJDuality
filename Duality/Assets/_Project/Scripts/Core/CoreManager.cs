using System;
using System.Collections;
using System.Collections.Generic;
using Duality.Player;
using UnityEngine;
using Magthylius;

namespace Duality.Core
{
    public class CoreManager : SoftSingletonPersistent<CoreManager>
    {
        [Header("References")] 
        public Camera mainCamera;
        public PlayerController player;
    
        [Header("Settings")]
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        private GameState _gameState;

        private void OnValidate()
        {
            mainCamera.backgroundColor = secondColor;
        }
        
        void Start()
        {
            _gameState = GameState.WaitForStart;
        }

        public static Color FirstColor => Instance.firstColor;
        public static Color SecondColor => Instance.secondColor;
        public static GameState CurrentState => Instance._gameState;
        public static Transform MainTransform => Instance.player.MainTR;

        public enum GameState
        {
            WaitForStart,
            Gameplay,
            WaitForEnd
        }
    }
}
