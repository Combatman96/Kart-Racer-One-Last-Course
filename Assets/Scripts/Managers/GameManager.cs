using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        EventManager.instance.onRaiseEvent += OnEventRaise;
    }

    private void OnEventRaise(string eventName, string p1, string p2, string p3, Vector3 v1, Vector3 v2)
    {
        switch (eventName)
        {
            case GameEvent.Empty_Event:
                Debug.Log(eventName);
                break;
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.onRaiseEvent -= OnEventRaise;
    }

    public void LoadScene(int buidldIndex)
    {
        SceneManager.LoadScene(buidldIndex, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U)) LoadScene(0);
        if(Input.GetKeyDown(KeyCode.I)) LoadScene(1);
    }
}

public enum GameFlow
{
    TitleScreen,
    OptionsScreen,
    KartSelectScreen
}

public enum SceneName
{

}
