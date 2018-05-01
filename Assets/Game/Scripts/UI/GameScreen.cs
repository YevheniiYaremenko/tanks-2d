using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameScreen : Screen
    {
        [SerializeField] Text scoreText;
        [SerializeField] Text timeText;
        [SerializeField] Text killsText;

        public void SetData(System.Action onShowInstructions, System.Action onEndGame)
        {
            this.onShowInstructions = onShowInstructions;
            this.onEndGame = onEndGame;
        }

        public void SetData(int score, int kills, float timer)
        {
            scoreText.text = string.Format("Score: {0}", score);
            killsText.text = string.Format("Kills: {0}", kills);
            timeText.text = string.Format("Time: {0}", Utils.TimeHelper.GetTime((int)timer));
        }

        System.Action onShowInstructions;
        public void ShowInstructions()
        {
            if (onShowInstructions != null)
            {
                onShowInstructions();
            }
        }

        System.Action onEndGame;
        public void EndGame()
        {
            if (onEndGame != null)
            {
                onEndGame();
            }
        }
    }
}