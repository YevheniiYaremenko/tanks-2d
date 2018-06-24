using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class GameScreen : Screen
    {
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI killsText;

        public void SetData(System.Action onEndGame)
        {
            this.onEndGame = onEndGame;
        }

        public void SetData(int score, int kills, float timer)
        {
            scoreText.SetText("Score: {0}", score);
            killsText.SetText("Kills: {0}", kills);
            timeText.SetText(string.Format("Time: {0}", TimeHelper.GetTime((int)timer)));
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