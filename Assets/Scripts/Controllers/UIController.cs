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

    public List<BaseUI> listScreen => GetComponentsInChildren<BaseUI>().ToList();

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
        }
    }


}
