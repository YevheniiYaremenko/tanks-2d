using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Game.Spawner;

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
        [SerializeField] SceneBuilder sceneBuilder;
        [Range(5, 50)] [SerializeField] int sceneSize = 10;
		[SerializeField] float timer = 120;

		bool game = false;
		Tank controllableTank;
		Model.Session session;

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

            sceneBuilder.BuildScene(sceneSize);
        }

		void Update()
		{
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

        #region UI

        public void StartGame(System.Type tankType)
        {
            startScreen.Hide();
            gameScreen.Show();

			controllableTank = Factory.TankFactory.Instance.GetItem<Tank>(tankType);
			controllableTank.transform.position = Vector3.zero;
			controllableTank.transform.eulerAngles = Vector3.zero;
			controllableTank.SetData(sceneSize);
			controllableTank.onDeath += (killing) => EndGame(false);

			Camera.main.GetComponent<Utils.CameraFollower>().SetTarget(controllableTank.transform);

            session = new Model.Session();

			EnemySpawner.Instance.SetData(
				Camera.main.orthographicSize * 3, 
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
			EnemySpawner.Instance.SetSpawnCenter(controllableTank.transform);

            game = true;
        }

        public void ShowInstructions()
        {
            instructionsScreen.Show();
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

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

        #endregion

        #region Level

        

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
			if (Input.GetKeyDown(KeyCode.X))
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
		}

		#endregion
    }
}
