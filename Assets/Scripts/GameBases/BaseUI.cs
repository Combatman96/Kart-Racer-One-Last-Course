using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BaseUI : MonoBehaviour
{
    [SerializeField] bool _isHideOtherScreen = false;

    public virtual void DoStart()
    {

    }
    
    public virtual void Show()
    {
        var UI = FindObjectOfType<BaseUIController>();
        var listScreen = UI.GetListScreens();
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

    public virtual void Hide()
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
