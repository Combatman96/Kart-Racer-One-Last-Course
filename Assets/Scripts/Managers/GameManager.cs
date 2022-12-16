using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public SceneName scene = SceneName.TitleScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        EventManager.instance.onRaiseEvent += OnEventRaise;
    }

    private void OnEventRaise(string eventName, params object[] p)
    {
        switch (eventName)
        {
            case GameEvent.Empty_Event:
                Debug.Log(eventName);
                break;
            case GameEvent.Kart_Selected:
                var kartSelected = (KartName)p[0];
                Debug.Log("KART SELECT : " + kartSelected.ToString());
                SetPlayerKart(kartSelected);
                break;
            case GameEvent.Track_Selected:
                var trackName = (SceneName)p[0];
                Debug.Log("TRACK SELECT : " + trackName.ToString());
                LoadScene(trackName);
                break;
            case GameEvent.GameMode_Selected:
                var mode = (GameMode)p[0];
                Debug.Log("GAME MODE : " + mode.ToString());
                SetGameMode(mode);
                break;
            case GameEvent.Game_Title_Screen:
                LoadScene(SceneName.TitleScreen);
                break;
        }
    }

    private void SetGameMode(GameMode mode)
    {
        var gameData = DataManager.instance.gameData;
        gameData.gameMode = mode;
    }

    private void SetPlayerKart(KartName kartName)
    {
        var dataGame = DataManager.instance.gameData;
        dataGame.playerKartName = kartName;
        switch (dataGame.gameMode)
        {
            case GameMode.Arcade:
                var arcadeConfig = DataManager.instance.config.arcadeConfig;
                SceneName firstScene = arcadeConfig.trackScenes[0].scene;
                LoadScene(firstScene);
                break;
            case GameMode.FreeRace:
                //Change to track select screen
                EventManager.instance.RaiseEvent(GameEvent.Select_Race_Track);
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

    public void ReLoadScene()
    {
        LoadScene(this.scene);
    }

    public void LoadScene(SceneName sceneName)
    {
        int buidldIndex = 0;
        var sceneConfig = DataManager.instance.config.sceneConfig;
        buidldIndex = sceneConfig.GetSceneBuildIndex(sceneName);
        SceneManager.LoadScene(buidldIndex, LoadSceneMode.Single);
        scene = sceneName;
        // Debug.Log("uiui");
    }

    public void OnRaceComplete()
    {
        var Data = DataManager.instance;
        if (Data.gameData.gameMode == GameMode.Arcade)
        {
            var nextScene = Data.config.arcadeConfig.GetNextScene(scene);
            LoadScene(nextScene);
        }
    }

    private void Update()
    {
        // TESTING
        if (Input.GetKeyDown(KeyCode.U)) LoadScene(0);
        if (Input.GetKeyDown(KeyCode.I)) LoadScene(1);
        if (Input.GetKeyDown(KeyCode.O))
        {
            DataManager.instance.gameData.playerKartName = KartName.Kardilact;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DataManager.instance.playerData.InitRecords();
            DataManager.instance.SavePlayerData();
        }
    }
}

public enum GameFlow
{
    TitleScreen = 0,
    OptionsScreen = 1,
    KartSelectScreen = 2,
    GamePlay = 3
}