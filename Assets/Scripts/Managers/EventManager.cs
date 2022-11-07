using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

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
