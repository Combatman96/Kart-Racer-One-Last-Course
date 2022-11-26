using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : BaseUI
{
    [SerializeField] private Button _arcadeBtn;
    [SerializeField] private Button _freeRaceBtn;
    [SerializeField] private Button _quitBtn;

    private MockupScene _mockupScene;

    // Start is called before the first frame update
    void Start()
    {
        SetEvent(_arcadeBtn, ()=> OnSelectGameMode(GameMode.Arcade));
        SetEvent(_freeRaceBtn, () => OnSelectGameMode(GameMode.FreeRace));

        SetEvent(_quitBtn, () =>
        {
            Application.Quit();
        });
    }

    void OnSelectGameMode(GameMode gameMode)
    {
        EventManager.instance.RaiseEvent(GameEvent.GameMode_Selected, new object[] { gameMode });
    }

    public override void Show()
    {
        base.Show();
        _mockupScene = FindObjectOfType<MockupScene>();
        _mockupScene.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        if(_mockupScene != null) _mockupScene.gameObject.SetActive(false);
    }
}
