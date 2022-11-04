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

    private void Awake()
    {
        raceDatas.Clear();
        foreach (Transform child in kartGroup)
        {
            KartName kartName = child.GetComponent<Kart>().kartName;
            raceDatas.Add(new RaceData(kartName));
        }
    }

    private void FixedUpdate()
    {
        GetKartsPositions();
        OnKartsCrossFinishLine();
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
            float distance = distanceAlongPath - finishLineDistanceAlongPath;
            if (distance < 0)
                distance = distanceAlongPath + finishLineDistanceAlongPath;
            raceDatas[i].distance = distance;
        }
    }

    private List<int> GetKartCrossFinishLine()
    {
        Transform raycast = finishLine.Find("Raycast");
        Collider[] colliders = Physics.OverlapBox(
            raycast.position,
            raycast.localScale / 2,
            Quaternion.identity,
            kartLayerMask
        );
        if (colliders.Length == 0)
            return null;
        List<int> listKartIndex = new List<int>();
        foreach (var collider in colliders)
        {
            var kartTransform = collider.transform.parent;
            int indexInKartGroup = kartTransform.GetSiblingIndex();
            listKartIndex.Add(indexInKartGroup);
        }
        return listKartIndex;
    }

    private void OnKartsCrossFinishLine()
    {
        var kartIndexs = GetKartCrossFinishLine();
        if(kartIndexs == null) return;
        var trackForward = finishLine.Find("TrackForward");
        Vector3 trackForwardDir = trackForward.localPosition.normalized;
        foreach(var kartIndex in kartIndexs)
        {
            Vector3 kartIncomingDir = (finishLine.position - kartGroup.GetChild(kartIndex).transform.position).normalized;
            float dot = Vector3.Dot(kartIncomingDir, trackForwardDir);
            if(dot > 0) 
            {
                raceDatas[kartIndex].distance += track.path.length;
            }
            else
            {
                raceDatas[kartIndex].distance -= track.path.length;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Draw finishLine Raycast
        Transform raycast = finishLine.Find("Raycast");
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(raycast.position, raycast.localScale);
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
}
