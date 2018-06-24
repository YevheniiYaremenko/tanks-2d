using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI
{
    public class InstructionsScreen : Screen
    {
        public void SetData(System.Action onBack)
        {
            this.onBack = onBack;
        }

        System.Action onBack;
        public void Back()
        {
            onBack?.Invoke();
        }
    }
}