using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using PathCreation;

public class PositionSystem : MonoBehaviour
{
    public PathCreator pathCreator;
    public Transform kartGroup;
    public Transform finishLine;
    public List<float> distances = new List<float>();

    private void Awake()
    {
        distances.Clear();
        foreach (Transform child in kartGroup)
        {
            distances.Add(0f);
        }
    }

    [Button]
    public void GetKartsPositions()
    {
        var path = pathCreator.path;
        Debug.Log(path.length);
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

    private void Update()
    {
        // GetKartsPositions();
    }
}
