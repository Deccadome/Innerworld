using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSwitcher : MonoBehaviour
{
    public Animator transition;
    public GameObject sceneWrapper;
    public bool worldSwitchEnabled = true;
    
    float transitionTime = 1f;
    public enum World
    {
        Outerworld,
        Innerworld
    }

    public World curWorld;
    public Scene curScene;
    Rigidbody2D playerRB;

    private void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && worldSwitchEnabled)
        {
            playerRB.Sleep();
            SwitchWorld();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && worldSwitchEnabled)
        {
            Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            playerRB.Sleep();
        }
    }

    public void SwitchWorld()
    {
        if(curWorld == World.Outerworld)
        {
            GameObject iwScene = GameObject.Find("IWSceneWrapper");
            if(iwScene == null)
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
        else if(curWorld == World.Innerworld)
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

    IEnumerator LoadWorld(World world)
    {
        string sceneName;

        if (world == World.Innerworld)
        {
            if (PlayerPrefs.HasKey("iwCurScene")) sceneName = PlayerPrefs.GetString("iwCurScene");
            else sceneName = "IW Hub";
        }
        else
        {
            if (PlayerPrefs.HasKey("owCurScene")) sceneName = PlayerPrefs.GetString("owCurScene");
            else sceneName = "OW Bedroom";
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        ToggleWorld(1 - world, false);

        asyncLoad.allowSceneActivation = true;

    }

    IEnumerator RestoreWorld(World world)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        ToggleWorld(world, true); // Enable target world
        ToggleWorld(1 - world, false); // Disable current world

    }

    void ToggleWorld(World world, bool state)
    {
        string wrapperName = "";
        if (world == World.Outerworld) wrapperName = "OWSceneWrapper";
        if (world == World.Innerworld) wrapperName = "IWSceneWrapper";

        try
        {
            Transform parent = GameObject.Find(wrapperName).transform;
            foreach(Transform child in parent)
            {
                child.gameObject.SetActive(state);
            }
        }
        catch (Exception ex){ Debug.LogException(ex, this); }

    }
}
