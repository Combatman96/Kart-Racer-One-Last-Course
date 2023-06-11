using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIHandler : MonoBehaviour
{
    private Kart car => GetComponent<Kart>();
    [SerializeField] private Transform waypointsGroup;
    [SerializeField] private List<FollowPoint> path = new List<FollowPoint>();

    [NaughtyAttributes.Button]
    private void SetFollowPath()
    {
        foreach(Transform waypoint in waypointsGroup)
        {
            FollowPoint pointPath = new FollowPoint(waypoint.position, waypoint.forward);
            path.Add(pointPath);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SetFollowPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

[System.Serializable]
public struct FollowPoint
{
    public Vector3 position;
    public Vector3 forward;

    public FollowPoint(Vector3 position, Vector3 forward)
    {
        this.position = position;
        this.forward = forward;
    }
}