using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController instance;
    
    private void Awake()
    {
        if(instance == null) instance = this;
    }

    public event Action<string, object[]> onRaiseEvent;
    public void RaiseEvent(string eventName, params object[] p)
    {
        if( onRaiseEvent != null)
        {
            onRaiseEvent(eventName, p);
        }
    }
}
