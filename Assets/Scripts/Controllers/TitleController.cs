using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.RaiseEvent(GameEvent.Main_Menu);
    }
}
