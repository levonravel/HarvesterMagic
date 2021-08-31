using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShinyBoxInteractive
{
    public class ScoreKeeper : MonoBehaviour
    {
        private static ScoreKeeper instance;
        public static ScoreKeeper Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<ScoreKeeper>();
                }
                return instance;
            }
        }
        public int PointsToWin;
        public Text ScoreText;
        public GameObject GameOverScreen;
        private int score;
        public int AdjustScore
        {
            set
            {
                score += value;
                if (score >= PointsToWin)
                {
                    GameOver();
                }
                ScoreText.text = score.ToString();
            }
        }
        public void GameOver()
        {
            Spawner.Instance.GameOver();
            GameOverScreen.SetActive(true);
        }
        public void ResetGame()
        {
            score = 0;
            ScoreText.text = "0";
            GameOverScreen.SetActive(false);
            Spawner.Instance.Start();            
        }
    }
}
