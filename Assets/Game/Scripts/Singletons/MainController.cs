using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Game.Spawner;
using Game.Utils;
using Unity.Mathematics;

namespace Game
{
    public class MainController : Singleton<MainController>
    {
		[Header("Data")]
        [SerializeField] Model.Data data;

        [Header("UI")]
        [SerializeField] UI.MenuScreen menuScreen;
        [SerializeField] UI.LevelSelectionScreen levelSelectionScreen;
        [SerializeField] UI.GameScreen gameScreen;
        [SerializeField] UI.EndGameScreen endGameScreen;
        [SerializeField] UI.InstructionsScreen instructionsScreen;
        [SerializeField] UI.Screen loadingScreen;
        List<UI.Screen> screens;

        [Header("Game Properties")]
		[SerializeField] float timer = 120;
        View.LevelView level;
		bool game = false;
		Tank controllableTank;
        System.Type selectedTankType;
		Model.Session session;

        SceneNavigationManager sceneNavigator;

        #region MonoBehaviour

        void Awake()
        {
            sceneNavigator = SceneNavigationManager.Instance;

            screens = new List<UI.Screen>()
            {
                menuScreen,
                levelSelectionScreen,
                gameScreen,
                endGameScreen,
                instructionsScreen,
                loadingScreen
            };

            gameScreen.SetData(() => EndGame(false));
            endGameScreen.SetData(RestartGame, () => LoadMenu(true));
            menuScreen.SetData(ExitGame, () => ShowScreen(instructionsScreen), () => ShowScreen(levelSelectionScreen));
            instructionsScreen.SetData(ShowMenuScreen);
            levelSelectionScreen.SetData(
                Factory.TankFactory.Instance.GetTypes(), 
                sceneNavigator.LevelScenes, 
                (type, level) => 
                {
                    selectedTankType = type;
                    LoadLevel(level);
                },
                ShowMenuScreen);
        }

        void Start()
        {
            LoadMenu(false);
        }

		void Update()
		{
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (game)
                {
                    EndGame(false);
                }
                else
                {
                    ExitGame();
                }
            }

            if (!game)
			{
				return;
			}

			ProcessTankControls();
			gameScreen.SetData(session.score, session.kills, timer);

            timer = math.max(0, timer - Time.deltaTime);
			if (timer == 0)
			{
				EndGame(true);
			}
        }

        #endregion

        #region Scene Navigation

        public void LoadMenu(bool showLoadingScreen = true)
        {
            if (showLoadingScreen)
            {
                loadingScreen.Show();
            }
            sceneNavigator.LoadMenu(() => 
            {
                ShowScreen(menuScreen);
                Sound.MusicManager.Play(Sound.MusicType.Menu);
            });
        }

        public void LoadLevel(string levelName)
        {
            loadingScreen.Show();
            sceneNavigator.LoadScene(levelName, () =>
            {
                ShowScreen(gameScreen);
                StartGame(selectedTankType);
            });
        }

        public void RestartGame()
        {
            LoadLevel(sceneNavigator.CurrentScene);
        }

        void ExitGame()
        {
            Application.Quit();
        }

        #endregion

        #region UI

        void ShowScreen(UI.Screen screen)
        {
            screens.ForEach(s => s.Hide());
            screen.Show();
        }

        void ShowMenuScreen()
        {
            ShowScreen(menuScreen);
        }

        #endregion

        #region Game

        public void StartGame(System.Type tankType)
        {
            Sound.MusicManager.Play(Sound.MusicType.Game);

            //level
            level = FindObjectOfType<View.LevelView>();

            //init player tank
            controllableTank = Factory.TankFactory.Instance.GetItem<Tank>(tankType);
            var playerSpawnPoint = level.PlayerSpawnPoints.Random();
            controllableTank.transform.position = playerSpawnPoint.position;
            controllableTank.transform.eulerAngles = playerSpawnPoint.eulerAngles;
            controllableTank.Init();
            controllableTank.onDeath += (killing) => EndGame(false);

            CameraFollower.Instance.SetTarget(controllableTank.transform);
            CameraFollower.Instance.Center();

            session = new Model.Session();

            //init enemies
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
            CameraFollower.Instance.Center();
            CameraFollower.Instance.SetTarget(null);
            EnemySpawner.Instance.Spawning = false;
            EnemySpawner.Instance.Reset();
            game = false;
            controllableTank.Move(0);
            controllableTank.Rotate(0);
            data.RegisterSession(session);

            endGameScreen.SetData(win, session.score, session.kills, data.Sessions, data.BestScore, data.MaxKills);
            ShowScreen(endGameScreen);
            Sound.MusicManager.Play(win ? Sound.MusicType.Win : Sound.MusicType.Lose);
        }

		void ProcessTankControls()
		{
			if (controllableTank == null || !game)
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
