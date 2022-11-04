using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using PathCreation;
using System.Linq;
using System;

public class PositionSystem : MonoBehaviour
{
    public PathCreator track;
    public Transform kartGroup;
    public Transform finishLine;
    public List<RaceData> raceDatas = new List<RaceData>();
    public LayerMask kartLayerMask;

    [Header("Debug")]
    public List<float> distancesDebug = new List<float>();
    public List<int> racePositionsDebug = new List<int>();
    public List<int> lapCompletedDebug = new List<int>();

    private List<float> _lapDistances = new List<float>();

    private void Awake()
    {
        raceDatas.Clear();
        _lapDistances.Clear();
        foreach (Transform child in kartGroup)
        {
            KartName kartName = child.GetComponent<Kart>().kartName;
            Debug.Log(kartName);
            raceDatas.Add(new RaceData(kartName, 0));
            _lapDistances.Add(0);
        }
    }

    private void FixedUpdate()
    {
        GetKartsPositions();

        //Debug
        DebugRacePos();
    }

    [Button]
    public void GetKartsPositions()
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

    public void OnKartsCrossFinishLine(int kartIndex)
    {
        Vector3 kartPos = kartGroup.GetChild(kartIndex).position;
        Vector3 inComingDir = (finishLine.position - kartPos).normalized;
        Vector3 trackForward = finishLine.Find("TrackForward").localPosition.normalized;
        float dot = Vector3.Dot(inComingDir, trackForward);
        if(dot < 0)
        {
            if(raceDatas[kartIndex].lapCompleted > 0)
            {
                raceDatas[kartIndex].lapCompleted -= 1;
                _lapDistances[kartIndex] = -1 * track.path.length * (raceDatas[kartIndex].lapCompleted);
            }               
        }
        else
        {
            raceDatas[kartIndex].lapCompleted += 1;
            _lapDistances[kartIndex] = -1 * track.path.length * (raceDatas[kartIndex].lapCompleted - 1);
        }
    }
    
    public void SetKartPositionInRace(int kartIndex)
    {
        var descending = new List<RaceData>(raceDatas);
        descending.Sort((a, b) => b.distance.CompareTo(a.distance));
        int pos = descending.IndexOf(raceDatas[kartIndex]);
        raceDatas[kartIndex].racePosition = pos;
    }

    public void SetKartsPositionsInRace()
    {
        for (int i = 0; i < kartGroup.childCount; i++)
        {
            SetKartPositionInRace(i);
        }
    }

    private void DebugRacePos()
    {
        distancesDebug = raceDatas.Select(x => x.distance).ToList();
        racePositionsDebug = raceDatas.Select(x => x.racePosition).ToList();
        lapCompletedDebug = raceDatas.Select(x => x.lapCompleted).ToList();
    }
}
