using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using NaughtyAttributes;

public class Wall : MonoBehaviour
{
    public PathCreator pathCreator;
    public float gap;
    public GameObject wallSegmentPrefab;

    [Button]
    public void CreateWalls()
    {
        var num = pathCreator.path.NumPoints;
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = pathCreator.path.GetPoint(i);
            Instantiate(wallSegmentPrefab, pos, Quaternion.identity, this.transform);
        }
    }
}
