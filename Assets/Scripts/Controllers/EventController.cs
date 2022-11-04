using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController current;
    
    private void Awake()
    {
        current = this;
    }

    public event Action<string, string, string, string> onRaiseEvent;
    public void RaiseEvent(string eventName, string p1 = "", string p2 = "", string p3 ="")
    {
        if( onRaiseEvent != null)
        {
            onRaiseEvent(eventName, p1, p2, p3);
        }
    }
}
