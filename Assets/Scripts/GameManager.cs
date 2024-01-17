using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Dome
{
    public class GameManager : MonoBehaviour
    {
        private const bool ON = true;
        private const bool OFF = false;

        private float wsTransitionTime = 1f;
        private float ngTransitionTime = 3f;

        public enum GameState
        {
            Outerworld,
            Innerworld,
            Paused,
            MainMenu
        }

        public GameState curState;
        private GameState prevState;
        public InputReader inputReader;
        private Animator transitionAnimator;
        private Canvas gmCanvas;

        public Scene curIWScene;
        public Scene curOWScene;
        public GameObject iwWrapper;
        public GameObject owWrapper;

        public bool worldSwitchEnabled = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            transitionAnimator = GetComponentInChildren<Animator>();
            gmCanvas = GetComponentInChildren<Canvas>();
            curState = GameState.MainMenu;
            inputReader.SetUI();

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
            gmCanvas.sortingOrder = 10;
            transitionAnimator.SetTrigger("newGame");
            yield return new WaitForSeconds(ngTransitionTime);

            // Prevent the menu from appearing again on scene load
            GameObject.Find("Main Menu Canvas").SetActive(false);

            // Load first scene and play opening animation
            SceneManager.LoadScene("OW Bedroom");
            ChangeState(GameState.Outerworld);
            transitionAnimator.SetTrigger("ngLoaded");
            inputReader.SetOW();
            worldSwitchEnabled = true;
        }

        // Pause handling
        private void Pause()
        {
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
        }

        private void Unpause()
        {
            ChangeState(prevState);
            Time.timeScale = 1f;
        }

        // Switching between IW and OW
        private void SwitchWorld()
        {
            Debug.Log("Switching world...");
            GameState otherWorld;
            if (worldSwitchEnabled)
            {
                Time.timeScale = 0f;
                if(curState == GameState.Outerworld)
                {
                    otherWorld = GameState.Innerworld;
                    if(curIWScene != null)
                    {
                        StartCoroutine(LoadWorld(GameState.Innerworld, otherWorld));
                        Debug.Log("Loading Innerworld");
                    }
                    else
                    {
                        StartCoroutine(RestoreWorld(GameState.Innerworld, otherWorld));
                        Debug.Log("Restoring Innerworld");
                    }
                    ChangeState(GameState.Innerworld);
                }
                else if (curState == GameState.Innerworld)
                {
                    otherWorld = GameState.Innerworld;
                    if (curOWScene != null)
                    {
                        StartCoroutine(LoadWorld(GameState.Innerworld, otherWorld));
                        Debug.Log("Loading Outerworld");
                    }
                    else
                    {
                        StartCoroutine(RestoreWorld(GameState.Innerworld, otherWorld));
                        Debug.Log("Restoring Outerworld");
                    }
                    ChangeState(GameState.Outerworld);
                }
            }
        }

        IEnumerator LoadWorld(GameState world, GameState otherWorld)
        {
            Scene loadScene;
            if (world == GameState.Outerworld) loadScene = curOWScene;
            else loadScene = curIWScene;

            AsyncOperation asyncLoad = AsyncLoadScene(loadScene.name);

            yield return SwitchWorldTransitionTrigger(world);

            ToggleWorld(otherWorld, OFF);
            asyncLoad.allowSceneActivation = true;
            Time.timeScale = 1;
            worldSwitchEnabled = true;
        }

        IEnumerator RestoreWorld(GameState world, GameState otherWorld)
        {
            yield return SwitchWorldTransitionTrigger(world);

            ToggleWorld(world, ON); // Enable target world
            ToggleWorld(otherWorld, OFF); // Disable current world
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
            if(gameState == GameState.Innerworld) { iwWrapper.SetActive(state); }
            else if(gameState == GameState.Outerworld) { owWrapper.SetActive(state); }
            else { Debug.Log("ToggleWorld: " + gameState + " not a valid state"); }
        }

        // Helper functions
        void ChangeState(GameState newState)
        {
            prevState = newState;
            curState = newState;
        }

        IEnumerator WaitForRealSeconds(float seconds)
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < seconds)
            {
                yield return null;
            }
        }

        AsyncOperation AsyncLoadScene(string sceneName, bool allowSceneActivation = false)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = allowSceneActivation;
            return asyncLoad;
        }
    }
}