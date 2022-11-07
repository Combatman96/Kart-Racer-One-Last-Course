using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    [SerializeField] bool _isHideOtherScreen = false;
    
    public void Show()
    {
        var listScreen = UIController.instance.listScreen;
        if(listScreen.Count == 0) return;

        foreach(var screen in listScreen)
        {
            screen.enabled = false;
            if(_isHideOtherScreen)
            {
                screen.Hide();
            }
        }
        this.gameObject.SetActive(true);
        this.enabled = true;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetEvent(Button btn, Action action)
    {
        if(btn == null) return;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => action?.Invoke());
    }

}
