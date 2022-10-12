using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    [Header("Suspensions")]
    [SerializeField] float _tireMass;
    [SerializeField] Transform _suspensions;

    [SerializeField] private float _length;
    [SerializeField] private float _restDistance;
    [SerializeField] private float _strength;
    [SerializeField] private float _damper;
    [SerializeField] private LayerMask _groundLayerMask;

    private Rigidbody _rigidbody => GetComponent<Rigidbody>();

    [Header("Steering")]
    [SerializeField][Range(0, 80)] float _maxSteeringAngle = 25f;
    [SerializeField] float _maxSteeringSpeed = 200f;
    [SerializeField] AnimationCurve _frontTiresGripCurve;
    [SerializeField] AnimationCurve _rearTiresGripCurve;
    [SerializeField][Range(0, 1)] float _frontTiresGripFactor;
    [SerializeField][Range(0, 1)] float _rearTiresGripFactor;

    [Header("Acceleration")]
    [SerializeField] float _topSpeed;
    [SerializeField] AnimationCurve _speedCurve;
    private float _accelerationInput = 0f;

    private void Update()
    {
        _accelerationInput = Input.GetAxis("Vertical") * _topSpeed;
        for (int i = 0; i < 2; i++)
            _suspensions.GetChild(i).localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * _maxSteeringAngle), _suspensions.GetChild(i).localEulerAngles.z);

    }


    private void FixedUpdate()
    {
        foreach (Transform suspension in _suspensions)
        {
            Debug.DrawRay(suspension.position, this.transform.up * -1 * _length, Color.green);

            RaycastHit hit;
            bool isRayHit = Physics.Raycast(suspension.position, this.transform.up * -1, out hit, _length, _groundLayerMask);
            if (isRayHit)
            {
                int wheelIndex = suspension.GetSiblingIndex();
                AddSuspensionForce(wheelIndex, hit);
                AddSteeringForce(wheelIndex);
                AddAccelerationForce(wheelIndex);
            }
        }
    }

    private void AddSuspensionForce(int wheelIndex, RaycastHit hit)
    {
        Transform suspension = _suspensions.GetChild(wheelIndex);
        Vector3 springDir = suspension.up;
        Vector3 springVel = _rigidbody.GetPointVelocity(suspension.position);
        float offet = _length - hit.distance;
        float velocity = Vector3.Dot(springDir, springVel);
        float force = (offet * _strength) - (velocity * _damper);
        _rigidbody.AddForceAtPosition(springDir * force, suspension.position);
    }

    private void AddSteeringForce(int wheelIndex, bool isAutoDrif = false)
    {
        var wheelTransform = _suspensions.GetChild(wheelIndex);
        Vector3 steerDir = wheelTransform.right;
        Vector3 tireWoldVel = _rigidbody.GetPointVelocity(wheelTransform.position);
        float steerVel = Vector3.Dot(steerDir, tireWoldVel);
        float tireGripFactor = 0;
        if (isAutoDrif)
        {
            float steerVelNormalized = Mathf.Clamp01(Mathf.Abs(steerVel) / _maxSteeringSpeed);
            tireGripFactor = (wheelTransform.GetSiblingIndex() < 2) ? _frontTiresGripCurve.Evaluate(steerVelNormalized) : _rearTiresGripCurve.Evaluate(steerVelNormalized);
        }
        else
        {
            tireGripFactor = (wheelTransform.GetSiblingIndex() < 2) ? _frontTiresGripFactor : _rearTiresGripFactor;
        }
        float desiredVelChange = -steerVel * tireGripFactor;
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
        _rigidbody.AddForceAtPosition(steerDir * _tireMass * desiredAccel, wheelTransform.position);
    }

    private void AddAccelerationForce(int wheelIndex)
    {
        var wheelTransform = _suspensions.GetChild(wheelIndex);
        Vector3 accelDir = wheelTransform.forward;
        if (_accelerationInput != 0f)
        {
            float carSpeed = Vector3.Dot(this.transform.forward, _rigidbody.velocity);
            float speedNormalized = Mathf.Clamp01(Mathf.Abs(carSpeed) / _topSpeed);
            float torque = _speedCurve.Evaluate(speedNormalized) * _accelerationInput;
            _rigidbody.AddForceAtPosition(accelDir * torque, wheelTransform.position);
        }
    }
}
