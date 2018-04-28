﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game
{
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
        [SerializeField]
        GameObject startScreen;
        [SerializeField] GameObject gameScreen;
        [SerializeField] GameObject instructionsScreen;
        [SerializeField] GameObject endGameScreen;
        List<GameObject> screens;

        [Header("Level")]
        [SerializeField] Transform sceneFloor;
        [SerializeField] Transform[] sceneBounds;
        [Range(5, 50)] [SerializeField] int sceneSize = 10;

		[Header("Tank")]
		[SerializeField] Weapon[] tankWeapons;

		bool game = false;
		Tank controllableTank;

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

            InitScene();
        }

		void Update()
		{
            if (!game)
			{
				return;
			}

			ProcessTankControls();
        }

        #region UI

        public void StartGame()
        {
            startScreen.SetActive(false);
            gameScreen.SetActive(true);

			controllableTank = Factory.TankFactory.Instance.GetRandom();
			controllableTank.transform.position = Vector3.zero;
			controllableTank.transform.eulerAngles = Vector3.zero;
			controllableTank.SetData(tankWeapons);

            game = true;
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

            game = false;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

        #endregion

        #region Level

        void InitScene()
        {
            sceneFloor.localScale = Vector3.one * (sceneSize + 10);
            sceneBounds[0].position = Vector2.up * sceneSize / 2f;
            sceneBounds[0].localScale = new Vector2(sceneSize + 1, 1);
            sceneBounds[1].position = Vector2.down * sceneSize / 2f;
            sceneBounds[1].localScale = new Vector2(sceneSize + 1, 1);
            sceneBounds[2].position = Vector2.left * sceneSize / 2f;
            sceneBounds[2].localScale = new Vector2(1, sceneSize + 1);
            sceneBounds[3].position = Vector2.right * sceneSize / 2f;
            sceneBounds[3].localScale = new Vector2(1, sceneSize + 1);
        }

        #endregion

		#region Tank

		void ProcessTankControls()
		{
			if (controllableTank == null)
			{
				return;
			}

            throw new System.NotImplementedException();
		}

		#endregion
    }
}
