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
    [SerializeField] private float _wheelOffset = 2.14f;
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
        if (Input.GetButtonDown("Jump"))
        {
            FlipCar();
        }
    }


    private void FixedUpdate()
    {
        foreach (Transform suspension in _suspensions)
        {
            Debug.DrawRay(suspension.position, this.transform.up * -1 * _length, Color.green);
            int wheelIndex = suspension.GetSiblingIndex();

            RaycastHit hit;
            bool isRayHit = Physics.Raycast(suspension.position, this.transform.up * -1, out hit, _length, _groundLayerMask);
            if (isRayHit)
            {
                AddSuspensionForce(wheelIndex, hit);
                AddSteeringForce(wheelIndex);
                if (wheelIndex > 1) AddAccelerationForce(wheelIndex);
                UpdateWheelPos(wheelIndex, hit.point, true);
            }
            else
            {
                UpdateWheelPos(wheelIndex, hit.point, false);
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

    private void FlipCar()
    {
        if (transform.up.y > 0) return;

        Vector3 axis = transform.up * -1;
        float torqueAmount = 120f;
        Vector3 torque = axis * torqueAmount;
        _rigidbody.AddForceAtPosition(torque, _suspensions.GetChild(0).position, ForceMode.Impulse);
        _rigidbody.AddForceAtPosition(torque, _suspensions.GetChild(2).position, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        foreach (Transform suspension in _suspensions)
        {
            Vector3 from = suspension.position;
            Vector3 to = from - new Vector3(0, _length, 0);
            Gizmos.DrawLine(from, to);
        }
    }

    private void UpdateWheelPos(int wheelIndex, Vector3 groundPos, bool isGrounded)
    {
        Transform wheel = _suspensions.GetChild(wheelIndex).GetChild(0);
        if (isGrounded)
        {
            wheel.position = groundPos + this.transform.up * _wheelOffset;
        }
        else
        {
            wheel.position = _suspensions.GetChild(wheelIndex).position - this.transform.up * (_length - _wheelOffset);
        }
    }
}
