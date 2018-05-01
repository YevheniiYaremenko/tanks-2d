using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StartScreen : Screen
    {
        [SerializeField] Button tankSelectionButton;

        public void SetData(System.Type[] types, System.Action<System.Type> onTankSelected)
        {
            foreach(var type in types)
            {
                var tankButton = Instantiate(tankSelectionButton, tankSelectionButton.transform.parent);
                tankButton.GetComponentInChildren<Text>().text = type.ToString();
                var t = type;
                tankButton.onClick.AddListener(() => onTankSelected(t));
                tankButton.gameObject.SetActive(true);
            }
        }
    }
}