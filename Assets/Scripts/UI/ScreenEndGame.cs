using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenEndGame : BaseUI
{
    [SerializeField] TextMeshProUGUI _resultTxt;
    [SerializeField] Image _resultImg;
    [SerializeField] Material[] _textMats;
    [SerializeField] WindowConfetti _confetti;
    [SerializeField] TextMeshProUGUI _raceTimeTxt;
    [SerializeField] TextMeshProUGUI _newRecordTxt;

    public override void DoStart()
    {
        ShowResult();
    }

    public void ShowResult()
    {
        var raceData = RaceController.instance.GetPlayerRaceData();
        int pos = raceData.racePosition;
        _resultTxt.text = pos.ToString();
        int index = Mathf.Clamp(pos - 1, 0, 3);
        _resultTxt.fontMaterial = _textMats[index];
        _resultImg.gameObject.SetActive(pos < 4);
        _raceTimeTxt.text = raceData.GetRaceTime();

        _confetti.gameObject.SetActive(pos < 4);
    }

    public void NewRecord(bool isNewRecord)
    {
        _newRecordTxt.gameObject.SetActive(isNewRecord);
    }
}
