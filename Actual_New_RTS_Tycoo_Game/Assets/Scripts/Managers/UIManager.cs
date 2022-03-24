using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] List<GameObject> _uIElements;

    void Awake()
    {
        Instance = this;
        _uIElements.ToggleUIElementVisibility(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var gmInst = GameManager.Instance;
            if (gmInst.IsGameInThisState(GameManager.GameStates.GamePaused))
            {
                gmInst.ChangeState(GameManager.GameStates.GameUnpaused);
            }
            else if (gmInst.IsGameInThisState(GameManager.GameStates.GameResumed) ||
                       gmInst.IsGameInThisState(GameManager.GameStates.GameStopped))
            {
                gmInst.ChangeState(GameManager.GameStates.GamePaused);
            }
        }
    }

    public void PauseButtonPausesGame()
    {
        Time.timeScale = 0;
        _uIElements.ToggleUIElementVisibility(true);
    }

    public void PauseButtonResumesGame()
    {
        Time.timeScale = 1;
        _uIElements.ToggleUIElementVisibility(false);
    }




}
