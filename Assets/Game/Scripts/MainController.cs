using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Game.Spawner;
using Game.Utils;

namespace Game
{
    public class MainController : MonoBehaviour
    {
        static MainController instance;
        public static MainController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MainController>();
                }
                return instance;
            }
        }

		[Header("Data")]
        [SerializeField] Model.Data data;

        [Header("UI")]
        [SerializeField] UI.StartScreen startScreen;
        [SerializeField] UI.Screen instructionsScreen;
        [SerializeField] UI.GameScreen gameScreen;
        [SerializeField] UI.EndGameScreen endGameScreen;
        List<UI.Screen> screens;

        [Header("Scene")]
		[SerializeField] float timer = 120;
        View.LevelView level;

		bool game = false;
		Tank controllableTank;
		Model.Session session;

        #region MonoBehaviour

        void Start()
        {
            screens = new List<UI.Screen>()
			{
				startScreen,
				gameScreen,
				instructionsScreen,
				endGameScreen
			};

            gameScreen.SetData(ShowInstructions, () => EndGame(false));
            endGameScreen.SetData(RestartGame);
            startScreen.SetData(Factory.TankFactory.Instance.GetTypes(), (type) => StartGame(type));
            startScreen.Show();
        }

		void Update()
		{
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (!game)
			{
				return;
			}

			ProcessTankControls();
			gameScreen.SetData(session.score, session.kills, timer);

            timer = Mathf.Max(0, timer - Time.deltaTime);
			if (timer == 0)
			{
				EndGame(true);
			}
        }

        #endregion

        #region UI

        public void ShowInstructions()
        {
            instructionsScreen.Show();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Level1");
        }

        #endregion

        #region Level

        public void StartGame(System.Type tankType)
        {
            startScreen.Hide();
            gameScreen.Show();

            //level
            level = FindObjectOfType<View.LevelView>();

            //controllable tank
            controllableTank = Factory.TankFactory.Instance.GetItem<Tank>(tankType);
            var playerSpawnPoint = level.PlayerSpawnPoints.Random();
            controllableTank.transform.position = playerSpawnPoint.position;
            controllableTank.transform.eulerAngles = playerSpawnPoint.eulerAngles;
            controllableTank.Init();
            controllableTank.onDeath += (killing) => EndGame(false);

            Camera.main.GetComponent<Utils.CameraFollower>().SetTarget(controllableTank.transform);

            session = new Model.Session();

            //enemies
            EnemySpawner.Instance.SetData(
                level.EnemySpawnPoints,
                (enemy) =>
                {
                    enemy.SetData(controllableTank != null ? controllableTank.transform : null);
                    enemy.onDeath += (killing) =>
                    {
                        if (killing)
                        {
                            session.score += enemy.KillBonus;
                            session.kills++;
                        }
                    };
                });
            EnemySpawner.Instance.Spawning = true;

            game = true;
        }

        public void EndGame(bool win)
        {
            Camera.main.GetComponent<Utils.CameraFollower>().SetTarget(null);
            EnemySpawner.Instance.Spawning = false;
            EnemySpawner.Instance.Reset();
            game = false;
            data.RegisterSession(session);

            screens.ForEach(s => s.Hide());
            endGameScreen.SetData(win, session.score, session.kills, data.Sessions, data.BestScore, data.MaxKills);
            endGameScreen.Show();
        }

        #endregion

		#region Tank

		void ProcessTankControls()
		{
			if (controllableTank == null)
			{
				return;
			}

			controllableTank.Move(Input.GetAxis("Vertical"));
            controllableTank.Rotate(Input.GetAxis("Horizontal"));
            controllableTank.ProcessTowerRotation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButtonDown(0))
			{
				controllableTank.Shoot();
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
                controllableTank.NextWeapon();
			}
            else if (Input.GetKeyDown(KeyCode.Q))
			{
                controllableTank.PreviousWeapon();
			}
            if (Input.GetMouseButton(0))
            {
                controllableTank.ShootAutomatically();
            }
		}

		#endregion
    }
}
