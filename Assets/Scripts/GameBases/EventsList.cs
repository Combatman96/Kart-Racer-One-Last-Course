using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventsList
{
}


public static class GameEvent
{
    public const string Empty_Event = "GameEvent.Empty_Event";
    public const string Kart_Selected = "GameEvent.Kart_Selected";
    public const string Track_Selected = "GameEvent.Track_Selected";
}

public static class EventGameplay
{
    public const string Empty_Event = "EventGameplay.Empty_Event";
    public const string Kart_Cross_Finish_Line = "EventGameplay.Kart_Cross_Finish_Line";
    public const string Change_State_Game = "EventGameplay.Change_State_Game";
    public const string Is_New_Record = "EventGameplay.Is_New_Record";
    public const string Player_Cross_FinishLine = "EventGameplay.Player_Cross_FinishLine";
}