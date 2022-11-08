using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenIngame : BaseUI
{
    [SerializeField] private TextMeshProUGUI _positionCounter;
    [SerializeField] private TextMeshProUGUI _totalKartCounter;
    [SerializeField] private TextMeshProUGUI _speedCounter;
    [SerializeField] private Image _speedMeter;

    private Kart _playerKart;

    public override void DoStart()
    {
        _playerKart = RaceController.instance.GetPlayerKart();

        int totalKart = RaceController.instance.GetTotalKart();
        _totalKartCounter.SetText("/" + totalKart);
    }

    void Update()
    {
        if (GameController.instance.gameState != GameState.GamePlay) return;

        UpdateSpeedCounter();
        UpdatePositionCounter();
    }

    public void UpdateSpeedCounter()
    {
        float speed = (int)_playerKart.GetSpeed() / 20;
        var speedProgress = Mathf.Clamp(speed, 50f, 500f);
        _speedMeter.fillAmount = speedProgress / 600f;
        _speedCounter.SetText(speed.ToString());
    }

    public void UpdatePositionCounter()
    {
        int playerPos = RaceController.instance.GetPlayerRacePosition();
        _positionCounter.SetText(playerPos.ToString());
    }
}
