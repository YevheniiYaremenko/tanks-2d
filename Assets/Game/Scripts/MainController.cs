using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
	static MainController instance;
	public static MainController Instance
	{
		get
		{
			if (instance = null)
			{
				instance = FindObjectOfType<MainController>();
			}
			return instance;
		}
	}

	[Header("UI")]
	[SerializeField] GameObject startScreen;
	[SerializeField] GameObject gameScreen;
	[SerializeField] GameObject instructionsScreen;
	[SerializeField] GameObject endGameScreen;
	List<GameObject> screens;

	void Start()
	{
		screens = new List<GameObject>()
		{
			startScreen,
			gameScreen,
			instructionsScreen,
			endGameScreen
		};
		startScreen.SetActive(true);
	}

	#region UI

	public void StartGame()
	{
        startScreen.SetActive(false);
		gameScreen.SetActive(true);
	}

	public void ShowInstructions()
	{
		instructionsScreen.SetActive(true);
	}

	public void HideInstructions()
	{
        instructionsScreen.SetActive(false);
	}

	public void EndGame()
	{
		screens.ForEach(s => s.SetActive(false));
		endGameScreen.SetActive(true);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("Main");
	}

	#endregion
}
