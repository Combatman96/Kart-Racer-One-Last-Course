using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIController : BaseUIController
{
    public static UIController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public Camera uiCamera;

    public List<BaseUI> listScreen;
    public ScreenStartGame screenStartGame;
    public ScreenIngame screenIngame;
    public PopupPauseGame popupPauseGame;
    public ScreenEndGame screenEndGame;

    private void Start()
    {
        EventController.instance.onRaiseEvent += OnEventRaise;

        listScreen = GetComponentsInChildren<BaseUI>(true).ToList();
        screenStartGame = GetComponentInChildren<ScreenStartGame>(true);
        screenIngame = GetComponentInChildren<ScreenIngame>(true);
        popupPauseGame = GetComponentInChildren<PopupPauseGame>(true);
        screenEndGame = GetComponentInChildren<ScreenEndGame>(true);
    }

    private void OnDestroy()
    {
        EventController.instance.onRaiseEvent -= OnEventRaise;
    }

    private void OnEventRaise(string eventName, params object[] p)
    {
        switch (eventName)
        {
            case EventGameplay.Empty_Event:
                Debug.Log(eventName);
                break;
            case EventGameplay.Change_State_Game:
                break;
            case EventGameplay.Is_New_Record:
                bool isNewRecord = (bool)p[0];
                screenEndGame.NewRecord(isNewRecord);
                break;
            case EventGameplay.Player_Cross_FinishLine:
                int lap = (int)p[0];
                screenIngame.SetLapCounter(lap);
                break;
        }
    }

    public void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartGame:
                screenStartGame.Show();
                screenStartGame.DoStart();
                break;
            case GameState.GamePlay:
                screenIngame.Show();
                screenIngame.DoStart();
                break;
            case GameState.Pause:
                popupPauseGame.Show();
                popupPauseGame.DoStart();
                break;
            case GameState.EndGame:
                screenEndGame.Show();
                screenEndGame.DoStart();
                break;
        }
    }

    public void ShowScreen(BaseUI screen)
    {
        screen.Show();
    }

    public override List<BaseUI> GetListScreens()
    {
        return listScreen;
    }
}
