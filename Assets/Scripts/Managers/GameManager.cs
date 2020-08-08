using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sweet_And_Salty_Studios
{
    public class GameManager : Singelton<GameManager>
    {
        #region VARIABLES

        public UnityEvent OnGameOver = new UnityEvent();
        public UnityEvent OnScoreIncreased = new UnityEvent();

        private Coroutine iStartGame_Coroutine = null;
        private Coroutine iQuitGame_Coroutine = null;
        private List<Coroutine> iSpawnLoop_Coroutines = new List<Coroutine>();

        private Camera mainCamera;
        private Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        private readonly float objectSpawnRate = 4f;     
        private float previousTimeScale;
        private bool isPaused;

        #endregion VARIABLES

        #region PROPERTIES

        public int Score
        {
            get;
            private set;
        }

        public bool IsGameRunning
        {
            get;
            private set;
        }

        public Player Player
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            mainCamera = GetComponentInChildren<Camera>();
            OnScoreIncreased.AddListener(
                () => 
                {
                    Score++;
                });
        }

        private void Start()
        {
            AudioManager.Instance.PlaySound(SoundType.BackgroundMusic);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void GameOver()
        {
            var activeGameModels = FindObjectsOfType<GameModel>();

            foreach(var gameModel in activeGameModels)
            {
                gameModel.OnDespawn();
            }

            if(iStartGame_Coroutine != null)
            {
                StopCoroutine(iStartGame_Coroutine);
                iStartGame_Coroutine = null;
            }

            foreach(var coroutine in iSpawnLoop_Coroutines)
            {
                StopCoroutine(coroutine);
            }

            iSpawnLoop_Coroutines.Clear();

            IsGameRunning = false;

            OnGameOver.Invoke();
        }

        public void StartGame()
        {
            if(iStartGame_Coroutine == null)
            {
                iStartGame_Coroutine = StartCoroutine(IStartGame());
            }
        }

        private void QuitGame()
        {
            if(iQuitGame_Coroutine == null)
            {
                iQuitGame_Coroutine = StartCoroutine(IQuitGame());
            }
        }

        public void PauseGame()
        {         
            previousTimeScale = Time.timeScale;
            Time.timeScale = isPaused ? 1 : 0;

            isPaused = !isPaused;
        }

        public Vector2 ConvertScreenToWorldPoint(Vector2 screenPoint)
        {
            return mainCamera.ScreenToWorldPoint(screenPoint);
        }

        private Vector2 GetRandomScreenToWorldPointFromBounds()
        {
            var randomSide = Random.Range(0, 4);

            switch(randomSide)
            {
                case 0:
                    return ConvertScreenToWorldPoint(
                        new Vector2(
                        Random.Range(0, Screen.width),
                        0));

                case 1:
                    return ConvertScreenToWorldPoint(
                        new Vector2(
                            Random.Range(0, Screen.width),
                            Screen.height));

                case 2:
                    return ConvertScreenToWorldPoint(
                        new Vector2(
                            0,
                            Random.Range(Screen.height, 0)));

                case 3:
                    return ConvertScreenToWorldPoint(
                        new Vector2(
                            Screen.width,
                            Random.Range(Screen.height, 0)));

                default:
                    return ConvertScreenToWorldPoint(Vector2.zero);
            }
        }

        private Vector2 GetRandomScreenToWorldPoint()
        {
            return ConvertScreenToWorldPoint(
                new Vector2(
                    Random.Range(0, Screen.width),
                    Random.Range(0, Screen.height))
                );
        }

        private void StartSpawnLoops()
        {
            iSpawnLoop_Coroutines.Add(StartCoroutine(
                IStartSpawnLoop<Enemy>(
                GetRandomScreenToWorldPointFromBounds(),
                Quaternion.identity,
                objectSpawnRate)));

            iSpawnLoop_Coroutines.Add(StartCoroutine(
                IStartSpawnLoop<Collectable>(
                GetRandomScreenToWorldPoint(),
                Quaternion.identity,
                objectSpawnRate,
                true)));
        }

        private IEnumerator IQuitGame()
        {
            yield return StartCoroutine(UI_Manager.Instance.UI_ScreenFader.IFade(1, 0.5f));

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private IEnumerator IStartGame()
        {
            IsGameRunning = true;
            Score = 0;

            Player = ObjectPoolManager.Instance.Spawn<Player>(
            Vector2.zero,
            Quaternion.identity,
            null);

            yield return new WaitUntil(() => InputManager.Instance.IsValidMousePositionDown);

            yield return new WaitForSeconds(objectSpawnRate);

            StartSpawnLoops();

            yield return new WaitUntil(() => Player.gameObject.activeSelf == false);

            GameOver();
        }

        private IEnumerator IStartSpawnLoop<T>(Vector2 spawnPosition, Quaternion spawnRotation, float spawnDelay = 1f, bool randomizePosition = false) where T : GameModel
        {
            while(IsGameRunning)
            {
                ObjectPoolManager.Instance.Spawn<T>(randomizePosition ? GetRandomScreenToWorldPoint() : spawnPosition, spawnRotation, null);

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}