using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class EndGameScreen : Screen
    {
        [SerializeField] TextMeshProUGUI endGameTitle;
        [SerializeField] TextMeshProUGUI levelInfoText;
        [TextArea] [SerializeField] string levelInfoFormat;

        public void SetData(System.Action onRestart, System.Action onMainMenu)
        {
            this.onRestart = onRestart;
            this.onMainMenu = onMainMenu;
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
            onRestart?.Invoke();
        }

        System.Action onMainMenu;
        public void MainMenu()
        {
            onMainMenu?.Invoke();
        }
    }
}