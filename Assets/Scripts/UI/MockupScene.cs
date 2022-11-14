using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockupScene : MonoBehaviour
{
    [SerializeField] private float _speedAngle = 15f;
    [SerializeField] private float _speedRoad = 5f;
    [SerializeField] private List<Transform> _wheels = new List<Transform>();
    [SerializeField] private Transform _road;

    // Update is called once per frame
    void Update()
    {
        //Spin the wheel
        foreach(var wheel in _wheels)
        {
            wheel.Rotate(new Vector3(_speedAngle, 0, 0), Space.Self);
        }
        _road.Rotate(new Vector3( 0, _speedRoad, 0), Space.Self);
    }
}
