using System.Collections;
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
        [SerializeField] GameObject startScreen;
        [SerializeField] GameObject instructionsScreen;
		[Header(" - Game Screen")]
        [SerializeField] GameObject gameScreen;
        [SerializeField] Text scoreText;
        [SerializeField] Text timeText;
		[SerializeField] Text killsText;
		[Header(" - End Game Screen")]
        [SerializeField] GameObject endGameScreen;
        [SerializeField] Text endGameTitle;
        [SerializeField] Text levelInfoText;
		[TextArea] [SerializeField] string levelInfoFormat;
        List<GameObject> screens;

        [Header("Scene")]
		[SerializeField] Transform environment;
        [SerializeField] SpriteRenderer groundSample;
		[SerializeField] Sprite[] groundSprites;
        [SerializeField] Transform[] sceneBounds;
        [Range(5, 50)] [SerializeField] int sceneSize = 10;
		[SerializeField] float timer = 120;

		bool game = false;
		Tank controllableTank;
		Model.Session session;

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
			UpdateGameScreen();

            timer = Mathf.Max(0, timer - Time.deltaTime);
			if (timer == 0)
			{
				EndGame();
			}
        }

        #region UI

        public void StartGame()
        {
            startScreen.SetActive(false);
            gameScreen.SetActive(true);

			controllableTank = Factory.TankFactory.Instance.GetRandom();
			controllableTank.transform.position = Vector3.zero;
			controllableTank.transform.eulerAngles = Vector3.zero;
			controllableTank.SetData(sceneSize);
			controllableTank.onDeath += (killing) => EndGame();

			Camera.main.GetComponent<Utils.CameraFollower>().SetTarget(controllableTank.transform);

            session = new Model.Session();

			Spawner.EnemySpawner.Instance.SetData(
				sceneSize + 5, 
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
			Spawner.EnemySpawner.Instance.Spawning = true;

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
            Camera.main.GetComponent<Utils.CameraFollower>().SetTarget(null);

            Spawner.EnemySpawner.Instance.Spawning = false;
			Spawner.EnemySpawner.Instance.Reset();

            game = false;

			data.RegisterSession(session);

            endGameTitle.text = controllableTank == null ? "You Lose!" : "You Win!";
			if (controllableTank != null)
			{
                levelInfoText.text = string.Format(
                levelInfoFormat,
                data.Sessions,
                session.score + (session.score == data.BestScore ? " (best score)" : ""),
                session.kills + (session.kills == data.MaxKills ? " (best score)" : ""));
			}
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

		void UpdateGameScreen()
		{
            scoreText.text = string.Format("Score: {0}", session.score);
            killsText.text = string.Format("Kills: {0}", session.kills);
            timeText.text = string.Format("Time: {0}", Utils.TimeHelper.GetTime((int)timer));
		}

        #endregion

        #region Level

        void InitScene()
        {
			var visibleSceneSize = sceneSize + 10;
			var groundOffset = - visibleSceneSize / 2f + .5f;
			for (int i = 0; i < visibleSceneSize; i++)
			{
				for (int j = 0; j < visibleSceneSize; j++)
				{
					Instantiate(
                        groundSample, 
						new Vector2(i + groundOffset, j + groundOffset),
						Quaternion.identity,
						environment)
						.sprite = groundSprites[Random.Range(0, groundSprites.Length)];
				}
			}

			var lineWidth = .1f;
            sceneBounds[0].position = Vector2.up * sceneSize / 2f;
            sceneBounds[0].localScale = new Vector2(sceneSize + lineWidth, lineWidth);
            sceneBounds[1].position = Vector2.down * sceneSize / 2f;
            sceneBounds[1].localScale = new Vector2(sceneSize + lineWidth, lineWidth);
            sceneBounds[2].position = Vector2.left * sceneSize / 2f;
            sceneBounds[2].localScale = new Vector2(lineWidth, sceneSize + lineWidth);
            sceneBounds[3].position = Vector2.right * sceneSize / 2f;
            sceneBounds[3].localScale = new Vector2(lineWidth, sceneSize + lineWidth);
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
