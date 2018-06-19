using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EndGameScreen : Screen
    {
        [SerializeField] Text endGameTitle;
        [SerializeField] Text levelInfoText;
        [TextArea] [SerializeField] string levelInfoFormat;

        public void SetData(System.Action onRestart)
        {
            this.onRestart = onRestart;
        }

        public void SetData(bool win, int score, int kills, int currentSession, int bestScore, int maxKills)
        {
            endGameTitle.text = win ? "You Win!" : "You Lose!";
            if (win)
            {
                levelInfoText.text = string.Format(
                levelInfoFormat,
                currentSession,
                score + (score == bestScore ? " (best score)" : ""),
                kills + (kills == maxKills ? " (best score)" : ""));
            }
        }

        System.Action onRestart;
        public void RestartGame()
        {
            if (onRestart != null)
            {
                onRestart();
            }
        }
    }
}