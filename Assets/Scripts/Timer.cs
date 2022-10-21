using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _time;
    [SerializeField] private Text _bestTime;

    private int _minuts;
    private float _seconds;

    private void Update()
    {
        _seconds += Time.deltaTime;
        if(_minuts >= PlayerPrefs.GetInt("minuts"))
        {
            PlayerPrefs.SetInt("minuts", _minuts);
            if(_seconds > PlayerPrefs.GetInt("second"))
            {
                PlayerPrefs.SetInt("second", (int)_seconds);
            }
        }
        if(_seconds > 60)
        {
            _minuts++;
            _seconds = 0;
            if(_minuts >= PlayerPrefs.GetInt("minuts"))
            {
                PlayerPrefs.SetInt("second", (int)_seconds);
            }
        }
        _time.text = $"Время: {_minuts}м {(int)_seconds}с"; 
        _bestTime.text = $"Лучшее время: {PlayerPrefs.GetInt("minuts")}м {PlayerPrefs.GetInt("second")}с";
    }
}
