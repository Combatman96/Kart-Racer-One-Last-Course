using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController current;
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        EventController.current.onRaiseEvent += OnEventRaise;
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
        EventController.current.onRaiseEvent -= OnEventRaise;
    }
}