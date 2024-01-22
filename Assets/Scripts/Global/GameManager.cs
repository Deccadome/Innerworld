using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace Dome
{
    public class GameManager : MonoBehaviour
    {
        private const bool ON = true;
        private const bool OFF = false;

        private float wsTransitionTime = 1f;
        private float ngTransitionTime = 3f;
        private float mmTransitionTime = 3f;

        public enum GameState
        {
            Outerworld,
            Innerworld,
            Paused,
            MainMenu
        }

        public GameState curState;
        [SerializeField]
        private GameState prevState;
        public InputReader inputReader;
        public GameObject pauseMenu;
        private Animator transitionAnimator;
        private Canvas gmCanvas;
        //private CameraController cam;

        public bool playerReady;

        public string curIWScene;
        public string curOWScene;
        public GameObject iwWrapper;
        public GameObject owWrapper;
        public GameObject owPlayer;
        public GameObject iwPlayer;

        public bool worldSwitchEnabled = false;

        public bool testing;

        private void Awake()
        {
            // If there is already a Game Manager, destroy it
            foreach (GameObject gameManager in GameObject.FindGameObjectsWithTag("GameManager"))
            {
                if (gameManager != gameObject) Destroy(gameObject);
            };
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            transitionAnimator = GetComponentInChildren<Animator>();
            gmCanvas = GetComponentInChildren<Canvas>();
            //cam = GetComponentInChildren<CameraController>();

            if (!testing) { 
                curState = GameState.MainMenu;
                inputReader.SetUI();
            }
            else
            {
                if(curState == GameState.Outerworld) inputReader.SetOW();
                else if(curState == GameState.Innerworld) inputReader.SetIW();
                Debug.Log("Current GameState: " + curState);
            }

            // Subscribe to input events
            inputReader.PauseEvent += Pause;
            inputReader.UnpauseEvent += Unpause;
            inputReader.SwitchWorldEvent += SwitchWorld;
        }

        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            StartCoroutine(NewGameCoroutine());
        }

        private IEnumerator NewGameCoroutine()
        {
            // Start menu fade animation
            playerReady = false;
            gmCanvas.sortingOrder = 10;
            transitionAnimator.SetTrigger("newGame");
            yield return WaitForRealSeconds(ngTransitionTime);

            // Load first scene and play opening animation
            curOWScene = "OW Bedroom";
            curIWScene = "IW Hub";
            SceneManager.LoadScene(curOWScene);
            while (!playerReady) yield return null;
            transitionAnimator.SetTrigger("ngLoaded");
            ChangeState(GameState.Outerworld);
            worldSwitchEnabled = true;
        }

        public void ReturnToMenu()
        {
            StartCoroutine(ReturnToMenuCoroutine());
        }

        private IEnumerator ReturnToMenuCoroutine()
        {
            transitionAnimator.SetTrigger("returnMenu");
            yield return WaitForRealSeconds(mmTransitionTime);

            // TODO: Save player progress

            // Destroy current OW/IW wrappers
            if(iwWrapper != null) Destroy(iwWrapper);
            if(owWrapper != null) Destroy(owWrapper);

            SceneManager.LoadScene("Main Menu");
            ChangeState(GameState.MainMenu);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            transitionAnimator.SetTrigger("enterMenu");
        }

        // Pause handling
        private void Pause()
        {
            if (curState == GameState.Outerworld || curState == GameState.Innerworld)
            {
                Debug.Log("Paused game");
                pauseMenu.SetActive(true);
                ChangeState(GameState.Paused);
                Time.timeScale = 0f;
            }
            else Debug.Log("Can't pause in state " + curState);
        }

        public void Unpause()
        {
            if(curState == GameState.Paused)
            {
                Debug.Log("Unpaused game");
                pauseMenu.SetActive(false);
                ChangeState(prevState);
                Time.timeScale = 1f;
            }
        }

        // Switching between IW and OW
        private void SwitchWorld()
        {
            Debug.Log("Switching world...");
            if (worldSwitchEnabled)
            {
                GameState otherWorld = (curState == GameState.Outerworld) ? GameState.Innerworld : GameState.Outerworld;

                Time.timeScale = 0f;
                playerReady = false;
                worldSwitchEnabled = false;

                StartCoroutine(HandleWorldSwitch(otherWorld, curState));
                ChangeState(otherWorld);
            }
        }

        IEnumerator HandleWorldSwitch(GameState targetWorld, GameState curWorld)
        {
            if(targetWorld == GameState.Outerworld && owWrapper == null
                || targetWorld == GameState.Innerworld && iwWrapper == null)
            {
                string loadScene = (targetWorld == GameState.Outerworld) ? curOWScene : curIWScene;
                AsyncOperation asyncLoad = AsyncLoadScene(loadScene);

                yield return SwitchWorldTransitionTrigger(targetWorld);
                ToggleWorld(curWorld, OFF);
                asyncLoad.allowSceneActivation = true;
            }
            else
            {
                yield return SwitchWorldTransitionTrigger(targetWorld);
                ToggleWorld(targetWorld, ON);
                ToggleWorld(curWorld, OFF);
            }

            while (!playerReady) yield return null;
            //cam.ChangeRoomBorders();
            transitionAnimator.SetTrigger((targetWorld == GameState.Outerworld) ? "I2O_Loaded" : "O2I_Loaded");
            yield return WaitForRealSeconds(wsTransitionTime);
            Time.timeScale = 1;
            worldSwitchEnabled = true;
        }

        public void LoadRoom(string sceneName)
        {
            StartCoroutine(LoadRoomCoroutine(sceneName));
        }

        private IEnumerator LoadRoomCoroutine(string sceneName)
        {
            worldSwitchEnabled = false;
            playerReady = false;
            Time.timeScale = 0;
            AsyncOperation asyncLoad = AsyncLoadScene(sceneName);
            transitionAnimator.SetTrigger("roomSwitch");
            yield return new WaitForSecondsRealtime(wsTransitionTime);
            string prevScene;
            if (curState == GameState.Innerworld)
            {
                prevScene = curIWScene;
                curIWScene = sceneName;
                yield return WaitForDestroy(iwWrapper);
            }
            else
            {
                prevScene = curOWScene;
                curOWScene = sceneName;
                yield return WaitForDestroy(owWrapper);
            }
            asyncLoad.allowSceneActivation = true;
            yield return new WaitForSecondsRealtime(wsTransitionTime);
            
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            RoomData roomData = GameObject.FindGameObjectWithTag("SceneWrapper").GetComponent<RoomData>();

            yield return roomData.WaitForRoomInit();
            Vector3 entrance = roomData.entrances[prevScene];
            playerTransform.position = entrance;
            //cam.ChangeRoomBorders();

            transitionAnimator.SetTrigger("roomSwitchLoaded");
            yield return new WaitForSecondsRealtime(wsTransitionTime);
            Time.timeScale = 1;
            worldSwitchEnabled = true;
        }

        IEnumerator SwitchWorldTransitionTrigger(GameState world)
        {
            string trigger = "";
            if (world == GameState.Outerworld) trigger = "I2O_Start";
            else if (world == GameState.Innerworld) trigger = "O2I_Start";

            transitionAnimator.SetTrigger(trigger);
            yield return WaitForRealSeconds(wsTransitionTime);
        }

        void ToggleWorld(GameState gameState, bool state)
        {
            if(gameState == GameState.Innerworld) { 
                iwWrapper.SetActive(state); 
                iwPlayer.SetActive(state);
            }
            else if(gameState == GameState.Outerworld) { 
                owWrapper.SetActive(state); 
                owPlayer.SetActive(state);
            }
            else { Debug.Log("ToggleWorld: " + gameState + " not a valid state"); }
        }

        // Helper functions
        public IEnumerator WaitForDestroy(GameObject obj)
        {
            Destroy(obj);
            while(obj != null) yield return null;
        }

        void ChangeState(GameState newState)
        {
            if (newState == GameState.MainMenu || newState == GameState.Paused) inputReader.SetUI();
            else if (newState == GameState.Outerworld) inputReader.SetOW();
            else if (newState == GameState.Innerworld) inputReader.SetIW();
            
            prevState = curState;
            curState = newState;
        }

        public IEnumerator WaitForRealSeconds(float seconds)
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < seconds)
            {
                yield return null;
            }
        }

        public AsyncOperation AsyncLoadScene(string sceneName, bool allowSceneActivation = false)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = allowSceneActivation;
            return asyncLoad;
        }
    }
}