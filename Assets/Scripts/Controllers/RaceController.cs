using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using PathCreation;
using System.Linq;
using System;

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
    private int _lapRequire;

    public static RaceController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    public void DoStart()
    {
        raceDatas.Clear();
        _lapDistances.Clear();
        _lapRequire = DataManager.instance.playerData.gameSetting.lapPerRace;

        _karts = kartGroup.GetComponentsInChildren<Kart>().ToList();
        var playerKartName = DataManager.instance.gameData.playerKartName;
        _playerKart = _karts.SingleOrDefault(x => x.kartName == playerKartName);
        _playerKart.transform.SetSiblingIndex(0);
        _karts = kartGroup.GetComponentsInChildren<Kart>().ToList();
        for (int i = 0; i < kartGroup.childCount; i++)
        {
            _karts[i].isPlayer = false;
            raceDatas.Add(new RaceData(_karts[i].kartName, 0));
            _lapDistances.Add(0);
            kartGroup.GetChild(i).transform.position = _kartPosGroup.GetChild(i).position;
            kartGroup.GetChild(i).transform.rotation = _kartPosGroup.GetChild(i).rotation;
            kartGroup.GetChild(i).gameObject.SetActive(i < DataManager.instance.gameData.maxKart);
        }

        StartCoroutine(CountDown(4));
    }

    public IEnumerator CountDown(int time)
    {
        yield return new WaitForSeconds(time);

        _playerKart.isPlayer = true;
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

    private void FixedUpdate()
    {
        if (GameController.instance.gameState == GameState.GamePlay)
        {
            GetKartsDistances();
            SetKartsPositionsInRace();
            MarkKartsPositions();
        }
    }

    private void MarkKartsPositions()
    {
        var worldPosList = new List<Vector3>();
        foreach (var x in racePositions)
        {
            var worldPos = _karts.SingleOrDefault(k => k.kartName == x).transform.position;
            worldPosList.Add(worldPos);
        }
        var playerPos = GetPlayerRacePosition();

        for (int i = 0; i < _markerGroup.childCount; i++)
        {
            var marker = _markerGroup.GetChild(i);
            if (i > worldPosList.Count - 1)
            {
                marker.gameObject.SetActive(false);
                continue;
            }
            marker.position = worldPosList[i] + _markerOffet;
            marker.forward = UIController.instance.uiCamera.transform.forward;
            if (i == playerPos - 1)
            {
                marker.gameObject.SetActive(false);
            }
            else
            {
                if (marker.gameObject.activeInHierarchy == false)
                {
                    marker.gameObject.SetActive(true);
                }
            }
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
        Vector3 kartPos = kartGroup.GetChild(kartIndex).position;
        Vector3 trackForward = finishLine.Find("TrackForward").localPosition.normalized;
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
        CheckEndRace();
    }

    private void CheckEndRace()
    {
        var playerRaceData = GetRaceData(_playerKart.kartName);
        if (playerRaceData.lap <= _lapRequire) return;

        foreach (Transform child in _markerGroup)
        {
            child.gameObject.SetActive(false);
        }
        int pos = GetRacePosition(_playerKart.kartName);
        SetEndTimes();
        EventController.instance.RaiseEvent(EventGameplay.Change_State_Game, new object[] { GameState.EndGame });
    }

    public void SetKartsPositionsInRace()
    {
        List<RaceData> DES = new List<RaceData>(raceDatas);
        DES.Sort((a, b) => b.distance.CompareTo(a.distance));
        racePositions = DES.Select(x => x.kartName).ToList();
        foreach (var data in raceDatas)
        {
            data.racePosition = GetRacePosition(data.kartName);
        }
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
        return racePositions.IndexOf(kartName) + 1;
    }

    public int GetPlayerRacePosition()
    {
        return GetRacePosition(_playerKart.kartName);
    }

    public Kart GetPlayerKart()
    {
        return _playerKart;
    }

    public int GetTotalKart()
    {
        return _lapDistances.Count;
    }
}
