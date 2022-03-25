using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameManager.GameStates prevState { get; private set; }

    [SerializeField] TMPro.TextMeshProUGUI goldText, stoneText;
    [SerializeField] int goldStart, stoneStart;
    public int goldBalance { get; private set; }
    public int stoneBalance { get; private set; }

    [SerializeField] List<GameObject> _uIElements;

    void Awake()
    {
        Instance = this;
        _uIElements.ToggleUIElementVisibility(false);
        goldBalance = goldStart;
        stoneBalance = stoneStart;
        UpdateUIValues();
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
                prevState = GameManager.Instance.GameState;
                gmInst.ChangeState(GameManager.GameStates.GamePaused);
            }
        }
    }
    #region Public Methods

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

    public void ToggleStopButton()
    {
        var gmInst = GameManager.Instance;
        if (gmInst.IsGameInThisState(GameManager.GameStates.GameResumed))
        {
            gmInst.ChangeState(GameManager.GameStates.GameStopped);
        }
        else if (gmInst.IsGameInThisState(GameManager.GameStates.GameStopped))
        {
            gmInst.ChangeState(GameManager.GameStates.GameResumed);
        }
    }

    public void BuyStructure(int goldCost, int stoneCost)
    {
        goldBalance -= goldCost;
        stoneBalance -= stoneCost;
        UpdateUIValues();
    }

    public void SellStructure(int goldCost, int stoneCost)
    {
        goldBalance += goldCost / 2;
        stoneBalance += stoneCost / 2;
        UpdateUIValues();
    }

    public bool IsStructureBuyable(int goldCost, int stoneCost)
    {
        if (goldBalance - goldCost < 0 || stoneBalance - stoneCost < 0) return false;
        else return true;
    }


    public void UpdateUIValues()
    {
        goldText.text = "" + goldBalance;
        stoneText.text = "" + stoneBalance;
    }

    #endregion




}
