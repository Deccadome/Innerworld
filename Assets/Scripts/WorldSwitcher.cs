using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dome
{
    public class WorldSwitcher : MonoBehaviour
    {
        public Animator transition;
        public GameObject sceneWrapper;
        public bool worldSwitchEnabled = true;
        public InputReader inputReader;

        float transitionTime = 1f;
        public enum World
        {
            Outerworld,
            Innerworld
        }

        public World curWorld;
        public Scene curScene;

        private void Start()
        {
            inputReader.SwitchWorldEvent += SwitchWorld;
        }

        void Update()
        {

        }

        public void SwitchWorld()
        {
            Debug.Log("IW: " + PlayerPrefs.GetString("iwCurScene"));
            Debug.Log("OW: " + PlayerPrefs.GetString("owCurScene"));

            if (worldSwitchEnabled)
            {
                Time.timeScale = 0;

                if (curWorld == World.Outerworld)
                {
                    GameObject iwScene = GameObject.Find("IWSceneWrapper");
                    if (iwScene == null)
                    {
                        StartCoroutine(LoadWorld(World.Innerworld));
                        Debug.Log("Loading Innerworld");
                    }
                    else
                    {
                        StartCoroutine(RestoreWorld(World.Innerworld));
                        Debug.Log("Restoring Innerworld");
                    }
                }
                else if (curWorld == World.Innerworld)
                {
                    GameObject owScene = GameObject.Find("OWSceneWrapper");
                    if (owScene == null)
                    {
                        StartCoroutine(LoadWorld(World.Outerworld));
                        Debug.Log("Loading Outerworld");
                    }
                    else
                    {
                        StartCoroutine(RestoreWorld(World.Outerworld));
                        Debug.Log("Restoring Outerworld");
                    }
                }
            }
        }

        IEnumerator LoadWorld(World world)
        {
            string sceneName;

            if (world == World.Innerworld)
            {
                if (PlayerPrefs.HasKey("iwCurScene")) sceneName = PlayerPrefs.GetString("iwCurScene");
                else
                {
                    sceneName = "IW Hub";
                    PlayerPrefs.SetString("iwCurScene", sceneName);
                }
            }
            else
            {
                if (PlayerPrefs.HasKey("owCurScene")) sceneName = PlayerPrefs.GetString("owCurScene");
                else
                {
                    sceneName = "OW Bedroom";
                    PlayerPrefs.SetString("owCurScene", sceneName);
                }
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            transition.SetTrigger("Start");

            yield return WaitForRealSeconds(transitionTime);


            ToggleWorld(1 - world, false);

            asyncLoad.allowSceneActivation = true;
            Time.timeScale = 1;
            worldSwitchEnabled = true;

        }

        IEnumerator RestoreWorld(World world)
        {
            transition.SetTrigger("Start");
            yield return WaitForRealSeconds(transitionTime);

            ToggleWorld(world, true); // Enable target world
            ToggleWorld(1 - world, false); // Disable current world
            Time.timeScale = 1;
            worldSwitchEnabled = true;
        }

        void ToggleWorld(World world, bool state)
        {
            string wrapperName = "";
            if (world == World.Outerworld) wrapperName = "OWSceneWrapper";
            if (world == World.Innerworld) wrapperName = "IWSceneWrapper";

            try
            {
                GameObject.Find(wrapperName).gameObject.SetActive(state);
            }
            catch (Exception ex) { Debug.LogException(ex, this); }

        }

        IEnumerator WaitForRealSeconds(float seconds)
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < seconds)
            {
                yield return null;
            }
        }
    }
}