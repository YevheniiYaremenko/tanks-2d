using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace Game.UI
{
    public class LevelSelectionScreen : Screen
    {
        [SerializeField] Toggle tankTypeToggle;
        [SerializeField] Toggle levelToggle;

        System.Action<System.Type, string> onGameSelected;
        System.Type selectedTankType;
        string selectedLevel;

        public void SetData(System.Type[] types, string[] levelNames, System.Action<System.Type, string> onGameSelected, System.Action onBack)
        {
            this.onGameSelected = onGameSelected;
            this.onBack = onBack;

            //tank types list
            for(int i = 0; i < types.Length; i++)
            {
                var toggle = Instantiate(tankTypeToggle, tankTypeToggle.transform.parent);
                toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(types[i].ToString().SeparateWords());

                var t = types[i];
                toggle.onValueChanged.AddListener((isOn) => {if (isOn) selectedTankType = t;});

                toggle.gameObject.SetActive(true);

                if (i == 0)
                {
                    toggle.isOn = true;
                }
            }

            //level list
            for (int i = 0; i < levelNames.Length; i++)
            {
                var toggle = Instantiate(levelToggle, levelToggle.transform.parent);
                toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(levelNames[i].SeparateWords(false));

                var n = levelNames[i];
                toggle.onValueChanged.AddListener((isOn) => { if (isOn) selectedLevel = n; });

                toggle.gameObject.SetActive(true);

                if (i == 0)
                {
                    toggle.isOn = true;
                }
            }
        }

        public void StartGame()
        {
            onGameSelected?.Invoke(selectedTankType, selectedLevel);
        }

        System.Action onBack;
        public void Back()
        {
            onBack?.Invoke();
        }
    }
}