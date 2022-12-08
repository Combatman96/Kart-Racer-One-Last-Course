using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIMainController : BaseUIController
{
    public static UIMainController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public Camera uiCamera;

    public List<BaseUI> listScreen => GetComponentsInChildren<BaseUI>(true).ToList();
    public ScreenMainMenu screenMainMenu => GetComponentInChildren<ScreenMainMenu>(true);
    public ScreenSelectKart screenSelectKart => GetComponentInChildren<ScreenSelectKart>(true);
    public ScreenTrackSelect screenTrackSelect => GetComponentInChildren<ScreenTrackSelect>(true);
    public ScreenRecord screenRecord => GetComponentInChildren<ScreenRecord>(true);

    private void Start()
    {
        EventManager.instance.onRaiseEvent += OnEventRaise;
    }

    private void OnDestroy()
    {
        EventManager.instance.onRaiseEvent -= OnEventRaise;
    }

    private void OnEventRaise(string eventName, params object[] p)
    {
        switch (eventName)
        {
            case EventGameplay.Empty_Event:
                Debug.Log(eventName);
                break;
            case GameEvent.GameTitleScreen:
                screenMainMenu.Show();
                screenMainMenu.DoStart();
                break;
            case GameEvent.GameMode_Selected:
                screenSelectKart.Show();
                screenSelectKart.DoStart();
                break;
            case GameEvent.Kart_Selected:
                var mode = DataManager.instance.gameData.gameMode;
                if (mode == GameMode.Arcade)
                {
                    return;
                }
                if (mode == GameMode.FreeRace)
                {
                    screenTrackSelect.Show();
                    screenTrackSelect.DoStart();
                }
                break;
            case GameEvent.Track_Selected:
                break;
            case GameEvent.Show_Record:
                screenRecord.Show();
                screenRecord.DoStart();
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
