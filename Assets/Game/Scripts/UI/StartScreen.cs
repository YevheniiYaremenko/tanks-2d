using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

                var title = type.ToString();
                title = System.IO.Path.GetExtension(title);
                title = title.Remove(0, 1);
                foreach(var ch in title.Where(x => x >= 'A' && x <= 'Z').Distinct())
                {
                    title = title.Replace(ch.ToString(), " " + ch);
                }

                tankButton.GetComponentInChildren<Text>().text = title;
                var t = type;
                tankButton.onClick.AddListener(() => onTankSelected(t));
                tankButton.gameObject.SetActive(true);
            }
        }
    }
}