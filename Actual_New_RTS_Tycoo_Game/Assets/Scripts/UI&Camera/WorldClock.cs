using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
public class WorldClock : MonoBehaviour
{
    [MinValue(6)] [MaxValue(18)]
    [SerializeField] int _startingHour;
    [MinValue(6)] [MaxValue(18)]
    [SerializeField] int _endingHour;
    [MinValue(0)]
    [SerializeField] float _timeSpeed;
    TextMeshProUGUI _textClock;
    float _timeInMinutes;
    

    void Awake()
    {
        _textClock = GetComponent<TextMeshProUGUI>();

        _timeInMinutes = _startingHour * 60;
    }

    void Update()
    {
        if (_timeInMinutes >= _endingHour * 60)
        {
            if (!GameManager.Instance.IsGameInThisState(GameManager.GameStates.GameOver)) 
                GameManager.Instance.ChangeState(GameManager.GameStates.GameOver);
        }

        _timeInMinutes += Time.deltaTime * _timeSpeed;
        DisplayTime(_timeInMinutes);
    }

    void DisplayTime(float timeToDisplay)
    {
        float hours = Mathf.FloorToInt(timeToDisplay / 60);
        float minutes = Mathf.FloorToInt(timeToDisplay % 60);
        _textClock.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }


}