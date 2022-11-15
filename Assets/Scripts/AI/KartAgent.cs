using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class KartAgent : Agent
{
    private Transform _checkPointGroup => FindObjectOfType<CheckPointGroup>().transform;
    private Kart _kart => GetComponentInParent<Kart>();

    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Transform _firstCheckPoint;

    private int _curCheckPointIndex = 0;
    private int _nextCheckPointIndex = 0;
    private int _checkPointCount;

    private void DoStart()
    {
        _kart.transform.position = _spawnPos.position + new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
        _kart.transform.forward = _spawnPos.forward;
        _kart.Stop();
    }

    public void InitCheckPoint()
    {
        _checkPointCount = _checkPointGroup.childCount;
        _nextCheckPointIndex = _firstCheckPoint.GetSiblingIndex() % _checkPointCount;
        _curCheckPointIndex = (_nextCheckPointIndex - 1) % _checkPointCount;
    }

    public void OnCheckPointReached(Transform checkPoint)
    {
        int index = checkPoint.GetSiblingIndex();
        if (index == _nextCheckPointIndex)
        {
            AddReward(1.5f);
            Debug.Log("right way");
            _curCheckPointIndex = _nextCheckPointIndex;
            _nextCheckPointIndex = (_nextCheckPointIndex + 1) % _checkPointCount;
        }
        else
        {
            AddReward(-1f);
            Debug.Log("wrong way");
        }
    }

    public void OnCheckPointStay()
    {
        AddReward(-0.1f);
    }

    public void OnWallEnter()
    {
        AddReward(-0.8f);
    }

    public void OnWallStay()
    {
        AddReward(-0.1f);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode begin");
        InitCheckPoint();
        // DoStart(); //Comment this if not training
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 checkPointForward = _checkPointGroup.GetChild(_nextCheckPointIndex).forward;
        float dotDir = Vector3.Dot(_kart.transform.forward, checkPointForward);
        sensor.AddObservation(dotDir);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float verticalInput = actions.ContinuousActions[0];
        float horizontalInput = actions.ContinuousActions[1];
        float drifInput = actions.DiscreteActions[0];
        bool isDrif = (drifInput > 0) ? true : false;

        _kart.InputHandler(verticalInput, horizontalInput, drifInput: isDrif);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float accelerationInput = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");
        bool drifInput = Input.GetKey(KeyCode.LeftShift);

        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = accelerationInput;
        continuousActions[1] = steeringInput;

        int dirfValue = (drifInput) ? 1 : 0;
        ActionSegment<int> discreateActions = actionsOut.DiscreteActions;
        discreateActions[0] = dirfValue;
    }
}
