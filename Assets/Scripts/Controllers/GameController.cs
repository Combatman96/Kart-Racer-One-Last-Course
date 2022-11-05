using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private void Awake()
    {
        instance = this;
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

    private void OnEventRaise(string eventName, string p1, string p2, string p3, Vector3 v1, Vector3 v2)
    {
        switch (eventName)
        {
            case EventGameplay.Empty_Event:
                Debug.Log(eventName);
                break;
            case EventGameplay.Kart_Cross_Finish_Line:
                RaceController.current.OnKartsCrossFinishLine(kartIndex: int.Parse(p1), inComingDir: v1);
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

