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
    public List<float> distances = new List<float>();
    public LayerMask kartLayerMask;

    private void Awake()
    {
        distances.Clear();
        foreach (Transform child in kartGroup)
        {
            distances.Add(0f);
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
            distances[i] = distance;
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
                distances[kartIndex] += track.path.length;
            }
            else
            {
                distances[kartIndex] -= track.path.length;
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

    public int GetKartPositionInRace(int kartIndex)
    {
        var descending = new List<float>(distances);
        descending.Sort((a, b) => b.CompareTo(a));
        int pos = descending.IndexOf(distances[kartIndex]);
        return pos;
    }

}
