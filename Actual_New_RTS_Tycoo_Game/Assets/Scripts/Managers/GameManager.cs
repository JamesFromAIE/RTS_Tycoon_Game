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
        ChangeState(GameStates.GenerateLevel);
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
            case GameStates.GenerateLevel:
                Debug.Log("Generated Level");
                GridManager.Instance.GenerateGrid();
                MoveCam.Instance.SetOrthoCamPosition();
                WorkerManager.Instance.SpawnWorkers();
                ChangeState(GameStates.GameResumed);
                break;
            case GameStates.GameStopped:
                Debug.Log("Game Time has been Stopped");
                Time.timeScale = 0;
                break;
            case GameStates.GameResumed:
                Debug.Log("Game Time has been Resumed");
                Time.timeScale = 1;
                break;
            case GameStates.GamePaused:
                Debug.Log("Game has been Paused");
                UIManager.Instance.PauseButtonPausesGame();
                break;
            case GameStates.GameUnpaused:
                Debug.Log("Game has been Unpaused");
                UIManager.Instance.PauseButtonResumesGame();
                ChangeState(UIManager.Instance.PrevState);
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
        GenerateLevel = 0,
        GameStopped = 1,
        GameResumed = 2,
        GamePaused = 3,
        GameUnpaused = 4,
        GameComplete = 5,
        GameOver = 6,
    }
}
