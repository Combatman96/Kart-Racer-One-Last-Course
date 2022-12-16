using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenStartGame : BaseUI
{
    [SerializeField] private TextMeshProUGUI _countDownTxt;
    [SerializeField] private Animation _countDownAnim;

    public override void DoStart()
    {
        CountDown();
    }

    public void CountDown()
    {
        _countDownAnim.Stop();
        _countDownAnim.Play();
    }

    public void UpdateCountDownText(string txt)
    {
        _countDownTxt.SetText(txt);
    }
}
