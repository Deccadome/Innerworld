using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Outerworld,
        Innerworld,
        Paused,
        MainMenu
    }

    public GameState curState;
    private GameState prevState;

    private void Update()
    {
        switch (curState)
        {
            case GameState.Outerworld:
                PauseManager();
                break;
            case GameState.Innerworld:
                PauseManager();
                break;
            case GameState.Paused:
                PauseManager();
                break;
            case GameState.MainMenu:
                break;
            default:
                Debug.LogWarning("Game State does not exist");
                break;
        }
    }

    void PauseManager()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(curState != GameState.Paused)
            {
                ChangeState(GameState.Paused);
                Time.timeScale = 0f;
                Debug.Log("Game paused");
            }
            // Unpause
            if(curState == GameState.Paused)
            {
                ChangeState(prevState);
                Time.timeScale = 1f;
                Debug.Log("Game unpaused");
            }
        }
    }

    void WorldSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(curState == GameState.Outerworld)
            {
                ChangeState(GameState.Innerworld);
            }
        }
    }

    void ChangeState(GameState newState)
    {
        curState = newState;
    }
}
