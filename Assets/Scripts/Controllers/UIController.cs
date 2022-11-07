using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    public List<BaseUI> listScreen => GetComponentsInChildren<BaseUI>(true).ToList();
    public ScreenStartGame screenStartGame => GetComponentInChildren<ScreenStartGame>(true);
    public ScreenIngame screenIngame => GetComponentInChildren<ScreenIngame>(true);
    public PopupPauseGame popupPauseGame => GetComponentInChildren<PopupPauseGame>(true);
    public ScreenEndGame screenEndGame => GetComponentInChildren<ScreenEndGame>(true);

    private void Start()
    {
        EventController.instance.onRaiseEvent += OnEventRaise;
    }

    private void OnEventRaise(string eventName, params object[] p)
    {
        switch (eventName)
        {
            case EventGameplay.Empty_Event:
                Debug.Log(eventName);
                break;
            case EventGameplay.Change_State_Game:
                GameState state = (GameState)p[0];
                OnGameStateChange(state);
                break;

        }
    }

    private void OnGameStateChange(GameState state)
    {
        switch(state)
        {
            case GameState.StartGame:
                screenStartGame.Show();
                break;
            case GameState.GamePlay:
                screenIngame.Show();
                break;
            case GameState.Pause:
                popupPauseGame.Show();
                break;
            case GameState.EndGame:
                screenEndGame.Show();
                break;
        }
    }


}
