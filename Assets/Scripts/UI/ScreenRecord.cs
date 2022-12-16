using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenRecord : BaseUI
{
    [SerializeField] List<TextMeshProUGUI> trackTxts = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> timeTxts = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> posTxts = new List<TextMeshProUGUI>();
    [SerializeField] Button backBtn;

    private List<Record> records = new List<Record>();

    // Start is called before the first frame update
    void Start()
    {
        SetEvent(backBtn, () => EventManager.instance.RaiseEvent(GameEvent.Main_Menu));
    }

    public override void DoStart()
    {
        GetRecords();
        ShowRecord();
    }

    private void GetRecords()
    {
        records = new List<Record>(DataManager.instance.playerData.records);
        records.RemoveAt(0);
    }

    private void ShowRecord()
    {
        for (int i = 0; i < records.Count - 1; i++)
        {
            trackTxts[i].text = records[i].track.ToString();
            timeTxts[i].text = records[i].GetRecordTime();
            posTxts[i].text = records[i].GetRecordPos().ToString();
        }
    }
}
