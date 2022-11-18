using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTrackSelect : BaseUI
{
    [SerializeField] Button _circusTrackBtn;
    [SerializeField] Button _spaceTrackBtn;
    [SerializeField] Button _cityTrackBtn;


    // Start is called before the first frame update
    void Start()
    {
        SetEvent(_circusTrackBtn, () => OnTrackSelect(SceneName.CircusTrack));
        SetEvent(_spaceTrackBtn, () => OnTrackSelect(SceneName.SpaceTrack));
        SetEvent(_cityTrackBtn, () => OnTrackSelect(SceneName.CityTrack));
    }

    private void OnTrackSelect(SceneName track)
    {
        EventManager.instance.RaiseEvent(GameEvent.Track_Selected, new object[] { track });
    }    
}
