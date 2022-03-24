using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameStates GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameStates.GenerateGrid);
    }

    public bool IsGameInThisState(GameStates checkState)
    {
        if (GameState == checkState) return true;
        else return false;
    }

    public void ChangeState(GameStates newState)
    {
        GameState = newState;
        switch(newState)
        {
            case GameStates.GenerateGrid:
                Debug.Log("Generated Grid");
                GridManager.Instance.GenerateGrid();
                break;
            case GameStates.GameStopped:
                Debug.Log("Game Time has been Stopped");
                MoveCam.Instance.SetOrthoCamPosition();
                break;
            case GameStates.GameResumed:
                Debug.Log("Game Time has Resumed");
                Time.timeScale = 1;
                break;
            case GameStates.GamePaused:
                Debug.Log("Game has been manually Paused");
                UIManager.Instance.PauseButtonPausesGame();
                break;
            case GameStates.GameUnpaused:
                Debug.Log("Game has been manually Resumed");
                UIManager.Instance.PauseButtonResumesGame();
                ChangeState(GameStates.GameResumed);
                break;
            case GameStates.GameOver:
                Debug.Log("Game is now Over");
                Time.timeScale = 0;
                break;
            case GameStates.GameComplete:
                Debug.Log("Game is now Complete");
                Time.timeScale = 0;
                break;
        }
    }

    public enum GameStates
    {
        GenerateGrid = 0,
        GameStopped = 1,
        GameResumed = 2,
        GamePaused = 3,
        GameUnpaused = 4,
        GameComplete = 5,
        GameOver = 6,
    }
}
