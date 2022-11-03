using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using PathCreation;

public class PositionSystem : MonoBehaviour
{
    public PathCreator pathCreator;
    public Transform kartGroup;
    public List<float> distances = new List<float>();

    [Button]
    public void GetKartsPositions()
    {
        var path = pathCreator.path;
        distances.Clear();
        foreach(Transform kartTransform in kartGroup)
        {
            float distance = path.GetClosestDistanceAlongPath(kartTransform.position);
            distances.Add(distance);
        }
    }
}
