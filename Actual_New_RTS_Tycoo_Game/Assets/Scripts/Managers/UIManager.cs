using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [HideInInspector] public UnityEvent UpdateBuildingPopulation;
    [HideInInspector] public UnityEvent UpdateCurrencyBalance;
    public GameManager.GameStates PrevState { get; private set; }

    [SerializeField] TMPro.TextMeshProUGUI _goldText, _stoneText, _populationText;
    [SerializeField] int _goldStart, _stoneStart, _populationStart;
    [MinValue(0)]
    public int GoldBalance { get; private set; }
    [MinValue(0)]
    public int StoneBalance { get; private set; }
    [MinValue(0)]
    public int PopulationBalance { get; private set; }

    [SerializeField] List<GameObject> _uIElements;

    void Awake()
    {
        Instance = this;
        UpdateBuildingPopulation = new UnityEvent();
        //UpdateCurrencyBalance = new UnityEvent();
        _uIElements.ToggleUIElementVisibility(false);
        GoldBalance = _goldStart;
        StoneBalance = _stoneStart;
        PopulationBalance = _populationStart;
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
                PrevState = GameManager.Instance.GameState;
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

    public void TriggerConstructionEvent()
    {
        PopulationBalance = 0;
        UpdateBuildingPopulation.Invoke();
        UpdateUIValues();
    }

    public void GainPopulation(int value)
    {
        PopulationBalance += value;
        //UpdateUIValues();
    }

    public void GainGold (int value)
    {
        GoldBalance += value;
        UpdateUIValues();
    }

    public void GainStone(int value)
    {
        StoneBalance += value;
        UpdateUIValues();
    }

    public void BuyStructure(int goldCost, int stoneCost)
    {
        GoldBalance -= goldCost;
        StoneBalance -= stoneCost;
        UpdateUIValues();
    }

    public void SellStructure(int goldCost, int stoneCost)
    {
        GoldBalance += goldCost / 2;
        StoneBalance += stoneCost / 2;
        UpdateUIValues();
    }

    public bool IsStructureBuyable(int goldCost, int stoneCost)
    {
        if (GoldBalance - goldCost < 0 || StoneBalance - stoneCost < 0) return false;
        else return true;
    }


    public void UpdateUIValues()
    {
        _goldText.text = "" + GoldBalance;
        _stoneText.text = "" + StoneBalance;
        _populationText.text = "" + PopulationBalance;
    }

    #endregion




}
