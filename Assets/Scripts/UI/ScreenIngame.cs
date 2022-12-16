using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenIngame : BaseUI
{

    [Header("Speed Counter")]
    [SerializeField] private TextMeshProUGUI _speedCounter;
    [SerializeField] private Image _speedMeter;

    [Header("Lap counter")]
    [SerializeField] private TextMeshProUGUI _currentLap;
    [SerializeField] private TextMeshProUGUI _totalLap;

    [SerializeField] Button pauseBtn;

    private Kart _playerKart;

    private void Start()
    {
        SetEvent(pauseBtn, () => OnPauseGame());
    }

    public override void DoStart()
    {
        _playerKart = RaceController.instance.GetPlayerKart();

        int totalKart = RaceController.instance.GetTotalKart();
        int totalLap = RaceController.instance.GetLapRequire();
        _totalLap.SetText("/" + totalLap);
        _currentLap.SetText("0");
    }

    void Update()
    {
        if (GameController.instance.gameState != GameState.GamePlay) return;

        UpdateSpeedCounter();
    }

    public void UpdateSpeedCounter()
    {
        float speed = (int)_playerKart.GetSpeed() / 20;
        var speedProgress = Mathf.Clamp(speed, 50f, 500f);
        _speedMeter.fillAmount = speedProgress / 600f;
        _speedCounter.SetText(speed.ToString());
    }


    public void SetLapCounter(int lap)
    {
        _currentLap.SetText(lap.ToString());
    }

    void OnPauseGame()
    {
        EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.Pause });
    }
}
