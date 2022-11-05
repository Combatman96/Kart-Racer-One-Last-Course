using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public bool isHideOtherScreen = false;

    public void Show()
    {
        var listScreen = UIController.instance.listScreen;
        if(listScreen.Count == 0) return;

        if(isHideOtherScreen)
        {
            foreach(var screen in listScreen)
            {
                screen.Hide();
            }
        }
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
