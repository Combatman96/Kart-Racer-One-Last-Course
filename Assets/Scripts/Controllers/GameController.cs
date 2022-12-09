using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameState gameState;

    private void Start()
    {
        EventController.instance.onRaiseEvent += OnEventRaise;
        EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.StartGame });
    }

    private void OnStartGame()
    {
        gameState = GameState.StartGame;

        RaceController.instance.DoStart();
    }

    private void OnPauseGame()
    {
        gameState = GameState.Pause;
        Time.timeScale = 0f;
    }

    private void OnPlayGame()
    {
        gameState = GameState.GamePlay;
        Time.timeScale = 1f;
    }

    private void OnEndGame()
    {
        gameState = GameState.EndGame;

        int playerPos = RaceController.instance.GetPlayerRacePosition();
        CheckRecord();
    }

    private void CheckRecord()
    {
        SceneName track = GameManager.instance.scene;
        RaceData playerRaceData = RaceController.instance.GetPlayerRaceData();
        bool isNewRecord = DataManager.instance.IsNewRecord(track, playerRaceData);
        if (isNewRecord)
        {
            DataManager.instance.UpdateRecord(track, playerRaceData);
            DataManager.instance.SavePlayerData();
        }
        EventController.instance.RaiseEvent(EventGameplay.Is_New_Record, new object[] { isNewRecord });

    }

    private void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartGame:
                OnStartGame();
                break;
            case GameState.GamePlay:
                OnPlayGame();
                break;
            case GameState.Pause:
                OnPauseGame();
                break;
            case GameState.EndGame:
                OnEndGame();
                break;
        }
    }

    private void OnEventRaise(string eventName, params object[] p)
    {
        switch (eventName)
        {
            case EventGameplay.Empty_Event:
                Debug.Log(eventName);
                break;
            case EventGameplay.Kart_Cross_Finish_Line:
                RaceController.instance.OnKartsCrossFinishLine(kartIndex: (int)p[0], inComingDir: (Vector3)p[1]);
                break;
            case EventGameplay.Change_State_Game:
                var newState = (GameState)p[0];
                OnGameStateChange(newState);
                break;
            case EventGameplay.Race_Done:
                OnRaceDone();
                break;
            case EventGameplay.Quit_Race:
                OnQuitRace();
                break;
            case EventGameplay.Restart_Race:
                OnRestartRace();
                break;
        }
    }

    private void OnRestartRace()
    {
        // EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.StartGame });
        GameManager.instance.ReLoadScene();
    }

    private void OnQuitRace()
    {
        GameManager.instance.LoadScene(SceneName.TitleScreen);
    }

    private void OnRaceDone()
    {
        GameManager.instance.OnRaceComplete();
    }

    private void OnDestroy()
    {
        EventController.instance.onRaiseEvent -= OnEventRaise;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.EndGame });
        }
    }
}

public enum GameState
{
    StartGame,
    GamePlay,
    Pause,
    EndGame,
}


