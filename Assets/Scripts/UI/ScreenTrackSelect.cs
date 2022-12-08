using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTrackSelect : BaseUI
{
    [SerializeField] Button _circusTrackBtn;
    [SerializeField] Button _spaceTrackBtn;
    [SerializeField] Button _cityTrackBtn;
    [SerializeField] Button _backBtn;

    public override void Show()
    {
        base.Show();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetEvent(_circusTrackBtn, () => OnTrackSelect(SceneName.RaceCircus));
        SetEvent(_spaceTrackBtn, () => OnTrackSelect(SceneName.SpaceCircus));
        SetEvent(_cityTrackBtn, () => OnTrackSelect(SceneName.CityCircus));
        SetEvent(_backBtn, () => OnBackBtnClick());
    }

    private void OnTrackSelect(SceneName track)
    {
        StartCoroutine(TrackLoadAfter(track, 0.5f));
    }

    IEnumerator TrackLoadAfter(SceneName track, float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.instance.RaiseEvent(GameEvent.Track_Selected, new object[] { track });
    }

    private void OnBackBtnClick()
    {
        EventManager.instance.RaiseEvent(GameEvent.Main_Menu);
    }
}
