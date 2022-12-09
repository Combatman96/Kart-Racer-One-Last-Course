using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPauseGame : BaseUI
{
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _restartRaceBtn;
    [SerializeField] private Button _quitRaceBtn;

    private void Start()
    {
        SetEvent(_resumeBtn, () => OnResumeBtnClick());
        SetEvent(_restartRaceBtn, () => OnRestartBtnClick());
        SetEvent(_quitRaceBtn, () => OnQuitRaceBtnClick());
    }

    void OnResumeBtnClick()
    {
        EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.GamePlay });
    }

    void OnRestartBtnClick()
    {
        EventController.instance.RaiseEvent(EventGameplay.Restart_Race);
    }

    void OnQuitRaceBtnClick()
    {
        EventController.instance.RaiseEvent(EventGameplay.Quit_Race);
    }
}
