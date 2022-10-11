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
    [SerializeField] float _maxSteeringSpeed;
    [SerializeField] AnimationCurve _frontTiresGrip;
    [SerializeField] AnimationCurve _rearTiresGrip;

    [Header("Acceleration")]
    [SerializeField] float _topSpeed;
    private float _accelerationInput;


    private void FixedUpdate()
    {
        foreach (Transform suspension in _suspensions)
        {
            Debug.DrawRay(suspension.position, this.transform.up * -1 * _length, Color.green);

            RaycastHit hit;
            bool isRayHit = Physics.Raycast(suspension.position, this.transform.up * -1, out hit, _length, _groundLayerMask);
            if (isRayHit)
            {
                // Suspension force
                Vector3 springDir = suspension.up;
                Vector3 springVel = _rigidbody.GetPointVelocity(suspension.position);
                float offet = _length - hit.distance;
                float velocity = Vector3.Dot(springDir, springVel);
                float force = (offet * _strength) - (velocity * _damper);
                _rigidbody.AddForceAtPosition(springDir * force, suspension.position);

                // Steering force
                Vector3 steerDir = suspension.right;
                Vector3 tireWoldVel = springVel;
                float steerVel = Vector3.Dot(steerDir, tireWoldVel);
                float steerVelNormalized = Mathf.Clamp01(Mathf.Abs(steerVel) / _maxSteeringSpeed);
                float tireGripFactor = (suspension.GetSiblingIndex() < 2) ? _frontTiresGrip.Evaluate(steerVelNormalized) : _rearTiresGrip.Evaluate(steerVelNormalized);
                float desiredVelChange = -steerVel * tireGripFactor;
                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
                _rigidbody.AddForceAtPosition(steerDir * _tireMass * desiredAccel, suspension.position);

                // Acceleration / Braking

            }
        }
    }
}
