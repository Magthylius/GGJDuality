using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magthylius;

namespace Duality.Core
{
    public class CoreManager : SoftSingletonPersistent<CoreManager>
    {
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        private GameState _gameState;
        
        // Start is called before the first frame update
        void Start()
        {
            _gameState = GameState.WaitForStart;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static Color FirstColor => Instance.firstColor;
        public static Color SecondColor => Instance.secondColor;
        public static GameState CurrentState => Instance._gameState;

        public enum GameState
        {
            WaitForStart,
            Gameplay,
            WaitForEnd
        }
    }
}
