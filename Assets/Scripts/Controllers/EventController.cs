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

    public event Action<string, string, string, string, Vector3, Vector3> onRaiseEvent;
    public void RaiseEvent(string eventName, string p1 = "", string p2 = "", string p3 ="", Vector3 v1 = default(Vector3), Vector3 v2 = default(Vector3))
    {
        if( onRaiseEvent != null)
        {
            onRaiseEvent(eventName, p1, p2, p3, v1, v2);
        }
    }
}
