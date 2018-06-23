using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI
{
    public class LevelSelectionScreen : Screen
    {
        [SerializeField] Toggle tankTypeToggle;
        [SerializeField] Toggle levelToggle;

        System.Action<System.Type, string> onGameSelected;
        System.Type selectedTankType;
        string selectedLevel;

        public void SetData(System.Type[] types, string[] levelNames, System.Action<System.Type, string> onGameSelected)
        {
            this.onGameSelected = onGameSelected;

            //tank types list
            for(int i = 0; i < types.Length; i++)
            {
                var toggle = Instantiate(tankTypeToggle, tankTypeToggle.transform.parent);
                toggle.GetComponentInChildren<Text>().text = types[i].ToString().SeparateWords();

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
                toggle.GetComponentInChildren<Text>().text = levelNames[i].SeparateWords(false);

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
    }
}