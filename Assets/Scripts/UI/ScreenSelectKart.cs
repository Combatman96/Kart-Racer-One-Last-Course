using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSelectKart : BaseUI
{
    [SerializeField] private KartName _kartSelect;
    [SerializeField] private TextMeshProUGUI _kartSelectTxt;
    [SerializeField] private Button _nextKartBtn;
    [SerializeField] private Button _previousKartBtn;
    private int _index = 0;
    private List<KartName> _listKartName;
    [SerializeField] Button _continueBtn;

    private KartDisplay kartDisplay => FindObjectOfType<KartDisplay>(true);
    [SerializeField] private float _rotateSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        SetEvent(_previousKartBtn, () => SetKartSelect(false));
        SetEvent(_nextKartBtn, ()=> SetKartSelect(true));
        SetEvent(_continueBtn, () => OnContinueBtnClick());
    }

    public override void Show()
    {
        base.Show();
        kartDisplay.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        kartDisplay.gameObject.SetActive(false);
    }

    public override void DoStart()
    {
        _index = 0;
        _listKartName = Enum.GetValues(typeof(KartName)).Cast<KartName>().ToList();
        _kartSelect = _listKartName[_index];
        SetKartSelect(isNext: false);
    }

    private void SetKartSelect(bool isNext)
    {
        _index = _index + ((isNext) ? +1 : -1);
        _index = Mathf.Clamp(_index, 0, _listKartName.Count - 1);
        _kartSelect = _listKartName[_index];
        for (int i = 0; i < kartDisplay.transform.childCount; i++)
        {
            var kart = kartDisplay.transform.GetChild(i);
            kart.gameObject.SetActive(i == _index);
        }
        _kartSelectTxt.text = _kartSelect.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        kartDisplay.transform.Rotate(new Vector3( 0, _rotateSpeed, 0), Space.Self);
    }

    void OnContinueBtnClick()
    {
        EventManager.instance.RaiseEvent(GameEvent.Kart_Selected, new object[] {
            _kartSelect
        });
    }
}
