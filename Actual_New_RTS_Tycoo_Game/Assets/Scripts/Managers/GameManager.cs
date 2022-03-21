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

    public void ChangeState(GameStates newState)
    {
        GameState = newState;
        switch(newState)
        {
            case GameStates.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameStates.GameStopped:
                MoveCam.Instance.SetOrthoCamPosition();
                //StructureManager.Instance.SpawnBuilding();
                //StructureManager.Instance.SpawnLandmark();
                break;
            case GameStates.GameResumed:
                break;
            case GameStates.GamePaused:
                break;
        }
    }

    public enum GameStates
    {
        GenerateGrid = 0,
        GameStopped = 1,
        GameResumed = 2,
        GamePaused = 3,

    }
}
