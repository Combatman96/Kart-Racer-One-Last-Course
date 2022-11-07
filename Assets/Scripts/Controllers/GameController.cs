using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private void Awake()
    {
        if(instance == null ) instance = this;
    }

    public GameState gameState;

    private void Start()
    {
        EventController.instance.onRaiseEvent += OnEventRaise;
    }

    public void StartGame()
    {
        gameState = GameState.StartGame;
    }

    public void PauseGame()
    {
        gameState = GameState.Pause;
    }

    public void PlayGame()
    {
        gameState = GameState.GamePlay;
    }

    public void EndGame()
    {
        gameState = GameState.EndGame;
    }

    private void OnEventRaise(string eventName, params object[] p)
    {
        switch (eventName)
        {
            case EventGameplay.Empty_Event:
                Debug.Log(eventName);
                break;
            case EventGameplay.Kart_Cross_Finish_Line:
                RaceController.instance.OnKartsCrossFinishLine(kartIndex: (int) p[0], inComingDir: (Vector3) p[1]);
                break;
        }
    }

    private void OnDestroy()
    {
        EventController.instance.onRaiseEvent -= OnEventRaise;
    }
}

public enum GameState
{
    StartGame,
    GamePlay,
    Pause,
    EndGame
}

