using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Duality.Core
{
    public class ScoreHandler : MonoBehaviour
    {
        public TextMeshProUGUI score;
        public string scorePrefix;
        public string highScorePrefix;

        private const string HighScoreKey = "Duality_HighScore";
        
        void Start()
        {
            CoreManager.Instance.PlayerDeathEvent += OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            int playerScore = CoreManager.Instance.EnemyDeathCount;
            bool newHighScore = CurrentHighScore() < playerScore;
            if (newHighScore) PlayerPrefs.SetInt(HighScoreKey, playerScore);
            
            score.text = 
                $"{scorePrefix} <b>{playerScore}</b> \n" +
                $"{highScorePrefix} <b>{CurrentHighScore()}</b> \n" +
                (newHighScore ? "New high score!" : String.Empty);
            
            int CurrentHighScore() => PlayerPrefs.GetInt(HighScoreKey);
        }
    }
}
