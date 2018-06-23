using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneNavigationManager : Singleton<SceneNavigationManager>
    {
		[SerializeField] string menuScene = "Menu";

		[SerializeField] string[] levelScenes;

		public string[] LevelScenes => levelScenes;
		public string CurrentScene { get { return SceneManager.GetActiveScene().name; } }

        public void LoadMenu(System.Action onLoaded = null)
		{
			LoadScene(menuScene, onLoaded);
		}

		public void LoadScene(string sceneName, System.Action onLoaded = null)
		{
			StartCoroutine(SceneLoadingCoroutine(sceneName, onLoaded));
		}

		IEnumerator SceneLoadingCoroutine(string sceneName, System.Action onLoaded = null)
		{
			var loading = SceneManager.LoadSceneAsync(sceneName);
			yield return new WaitUntil(() => loading.isDone);

			onLoaded?.Invoke();
		}
    }
}
