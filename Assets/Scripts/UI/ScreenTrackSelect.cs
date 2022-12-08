using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTrackSelect : BaseUI
{
    [SerializeField] Button _circusTrackBtn;
    [SerializeField] Button _spaceTrackBtn;
    [SerializeField] Button _cityTrackBtn;

    public override void Show()
    {
        base.Show();
        Debug.Log("track select");
    }

    // Start is called before the first frame update
    void Start()
    {
        SetEvent(_circusTrackBtn, () => OnTrackSelect(SceneName.RaceCircus));
        SetEvent(_spaceTrackBtn, () => OnTrackSelect(SceneName.SpaceCircus));
        SetEvent(_cityTrackBtn, () => OnTrackSelect(SceneName.CityCircus));
    }

    private void OnTrackSelect(SceneName track)
    {
        EventManager.instance.RaiseEvent(GameEvent.Track_Selected, new object[] { track });
    }
}
