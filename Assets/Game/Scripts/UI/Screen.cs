using UnityEngine;

namespace Game.UI
{
    ///<summary>
	/// UI screen class
	///</summary>
    public class Screen : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
