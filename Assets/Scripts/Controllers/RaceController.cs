using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using PathCreation;
using System.Linq;
using System;
using Cinemachine;
using System.Threading.Tasks;

public class RaceController : MonoBehaviour
{
    public PathCreator track;
    public Transform kartGroup;
    public Transform finishLine;
    public List<RaceData> raceDatas = new List<RaceData>();
    public List<KartName> racePositions = new List<KartName>();
    [SerializeField] private Transform _kartPosGroup;
    private Kart _playerKart;
    private List<Kart> _karts = new List<Kart>();
    [SerializeField] Transform _markerGroup;
    [SerializeField] Vector3 _markerOffet;

    private List<float> _lapDistances = new List<float>();
    [SerializeField] private int _lapRequire;

    [SerializeField] private CinemachineVirtualCamera vCamera;

    public static RaceController instance;

    private List<KartName> clearRaceList = new List<KartName>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    public void DoStart()
    {
        // Debug.Log("Start race");
        clearRaceList.Clear();
        raceDatas.Clear();
        _lapDistances.Clear();
        _lapRequire = DataManager.instance.playerData.gameSetting.lapPerRace;

        _karts = kartGroup.GetComponentsInChildren<Kart>().ToList();
        var playerKartName = DataManager.instance.gameData.playerKartName;
        _playerKart = _karts.SingleOrDefault(x => x.kartName == playerKartName);
        _playerKart.transform.SetSiblingIndex(0);

        vCamera.Follow = _playerKart.transform;
        vCamera.LookAt = _playerKart.transform;

        _karts = kartGroup.GetComponentsInChildren<Kart>().ToList();
        for (int i = 0; i < kartGroup.childCount; i++)
        {
            raceDatas.Add(new RaceData(_karts[i].kartName, 0));
            _karts[i].SetIsPlayer(false);
            _lapDistances.Add(0);
            kartGroup.GetChild(i).transform.position = _kartPosGroup.GetChild(i).position;
            kartGroup.GetChild(i).transform.rotation = _kartPosGroup.GetChild(i).rotation;
            kartGroup.GetChild(i).gameObject.SetActive(i < DataManager.instance.gameData.maxKart);
        }
        _playerKart.SetIsPlayer(true);
        CoundownTask();
    }

    public async void CoundownTask()
    {
        await Task.Delay(4000);
        SetStartTimes();
        EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, GameState.GamePlay);
    }


    private void SetStartTimes()
    {
        foreach (var data in raceDatas)
        {
            data.SetStartRaceTicks(DateTime.Now);
        }
    }

    private void SetEndTimes()
    {
        foreach (var data in raceDatas)
        {
            data.SetEndRaceTicks(DateTime.Now);
        }
    }

    [Button]
    public void GetKartsDistances()
    {
        var path = track.path;
        for (int i = 0; i < kartGroup.childCount; i++)
        {
            Vector3 kartPos = kartGroup.GetChild(i).position;
            float distanceAlongPath = path.GetClosestDistanceAlongPath(kartPos);
            float finishLineDistanceAlongPath = path.GetClosestDistanceAlongPath(
                finishLine.position
            );
            float distance = distanceAlongPath - finishLineDistanceAlongPath + _lapDistances[i];
            if (distance < 0)
                distance = distanceAlongPath + finishLineDistanceAlongPath + _lapDistances[i];
            raceDatas[i].distance = distance;
        }
    }

    public void OnKartsCrossFinishLine(int kartIndex, Vector3 inComingDir)
    {
        if (GameController.instance.gameState != GameState.GamePlay) return;

        Vector3 kartPos = kartGroup.GetChild(kartIndex).position;
        Vector3 trackForward = finishLine.Find("TrackForward").localPosition.normalized;
        Debug.Log("Hi");
        float dot = Vector3.Dot(inComingDir, trackForward);
        if (dot < 0)
        {
            if (raceDatas[kartIndex].lap > 0)
            {
                raceDatas[kartIndex].lap -= 1;
            }
        }
        else
        {
            raceDatas[kartIndex].lap += 1;
        }
        _lapDistances[kartIndex] = track.path.length * (raceDatas[kartIndex].lap);
        UpdateKartLap(kartIndex);
        CheckEndRace();
    }

    private void UpdateKartLap(int index)
    {
        var raceData = GetRaceData(_karts[index].kartName);
        if (raceData.lap == _lapRequire + 1)
        {
            clearRaceList.Add(raceData.kartName);
            raceData.racePosition = GetRacePosition(raceData.kartName);
        };
        if (!_karts[index].isPlayer) return;
        EventController.instance.RaiseEvent(EventGameplay.Player_Cross_FinishLine, new object[] { raceData.lap });
    }

    private void CheckEndRace()
    {
        var playerRaceData = GetRaceData(_playerKart.kartName);
        if (playerRaceData.lap <= _lapRequire) return;

        // foreach (Transform child in _markerGroup)
        // {
        //     child.gameObject.SetActive(false);
        // }
        // int pos = GetRacePosition(_playerKart.kartName);
        SetEndTimes();
        EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.EndGame });
    }

    public RaceData GetRaceData(KartName kartName)
    {
        return raceDatas.Single(x => x.kartName == kartName);
    }

    public RaceData GetPlayerRaceData()
    {
        return GetRaceData(_playerKart.kartName);
    }

    public int GetRacePosition(KartName kartName)
    {
        return clearRaceList.IndexOf(kartName) + 1;
    }

    public int GetPlayerRacePosition()
    {
        // return GetRacePosition(_playerKart.kartName);
        return clearRaceList.IndexOf(_playerKart.kartName) + 1;
    }

    public Kart GetPlayerKart()
    {
        return _playerKart;
    }

    public int GetTotalKart()
    {
        return _lapDistances.Count;
    }

    public int GetLapRequire()
    {
        return _lapRequire;
    }
}
