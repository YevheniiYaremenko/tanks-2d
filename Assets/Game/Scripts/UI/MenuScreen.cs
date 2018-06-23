using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI
{
    public class MenuScreen : Screen
    {
        System.Action onExitGame;
        System.Action onSelectLevel;

        public void SetData(System.Action onExitGame, System.Action onSelectLevel)
        {
            this.onExitGame = onExitGame;
            this.onSelectLevel = onSelectLevel;
        }

        public void ExitGame()
        {
            onExitGame?.Invoke();
        }

        public void SelectLevel()
        {
            onSelectLevel?.Invoke();
        }
    }
}