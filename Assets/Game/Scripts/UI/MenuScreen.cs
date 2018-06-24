using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI
{
    public class MenuScreen : Screen
    {
        System.Action onExitGame;
        System.Action onShowControls;
        System.Action onSelectLevel;

        public void SetData(System.Action onExitGame, System.Action onShowControls, System.Action onSelectLevel)
        {
            this.onExitGame = onExitGame;
            this.onShowControls = onShowControls;
            this.onSelectLevel = onSelectLevel;
        }

        public void ExitGame()
        {
            onExitGame?.Invoke();
        }

        public void ShowControls()
        {
            onShowControls?.Invoke();
        }

        public void SelectLevel()
        {
            onSelectLevel?.Invoke();
        }
    }
}