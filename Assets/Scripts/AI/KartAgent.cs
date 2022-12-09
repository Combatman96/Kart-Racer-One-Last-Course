using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class KartAgent : Agent
{
    [SerializeField] private Transform _checkPointGroup;
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
        try
        {
            _checkPointCount = _checkPointGroup.childCount;
            _nextCheckPointIndex = _firstCheckPoint.GetSiblingIndex() % _checkPointCount;
            _curCheckPointIndex = (_nextCheckPointIndex - 1) % _checkPointCount;
        }
        catch (System.Exception)
        {

            // throw;
        }
    }

    public void OnCheckPointReached(Transform checkPoint)
    {
        int index = checkPoint.GetSiblingIndex();
        if (index == _nextCheckPointIndex)
        {
            AddReward(1.5f);
            Debug.Log("right way");
            try
            {
                _curCheckPointIndex = _nextCheckPointIndex;
                _nextCheckPointIndex = (_nextCheckPointIndex + 1) % _checkPointCount;
            }
            catch (System.Exception)
            {

                // throw;
            }
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
        try
        {
            Vector3 checkPointForward = _checkPointGroup.GetChild(_nextCheckPointIndex).forward;
            float dotDir = Vector3.Dot(_kart.transform.forward, checkPointForward);
            sensor.AddObservation(dotDir);
        }
        catch (System.Exception)
        {
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float acceleration = actions.DiscreteActions[0] - 1f;
        float steeringInput = actions.DiscreteActions[1] - 1f;

        int drifInput = actions.DiscreteActions[2];
        bool isDrif = (drifInput > 0) ? true : false;

        if (!_kart.isPlayer)
            _kart.InputHandler(acceleration, steeringInput, drifInput: isDrif);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int accelerationInput = 1;
        int steeringInput = 1;

        if (Input.GetKey(KeyCode.UpArrow)) accelerationInput = 2;
        if (Input.GetKey(KeyCode.DownArrow)) accelerationInput = 0;

        if (Input.GetKey(KeyCode.RightArrow)) steeringInput = 2;
        if (Input.GetKey(KeyCode.LeftArrow)) steeringInput = 0;

        int drifInput = (Input.GetKey(KeyCode.LeftShift)) ? 1 : 0;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = accelerationInput;
        discreteActions[1] = steeringInput;
        discreteActions[2] = drifInput;
    }
}
