using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : BaseUI
{
    [SerializeField] private Button _arcadeBtn;
    [SerializeField] private Button _freeRaceBtn;
    [SerializeField] private Button _quitBtn;

    private MockupScene _mockupScene => FindObjectOfType<MockupScene>();

    // Start is called before the first frame update
    void Start()
    {
        SetEvent(_quitBtn, () =>
        {
            Application.Quit();
        });
    }

    public override void Show()
    {
        base.Show();
        _mockupScene.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        _mockupScene.gameObject.SetActive(false);
    }
}
