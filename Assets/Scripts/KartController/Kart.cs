using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 200f;
    [SerializeField]
    private float steerAmount = 10f;
    [SerializeField]
    private float drif = 30f;
    [SerializeField]
    private float acceleration = 360;
    [SerializeField]
    private float steering = 0.5f;

    private Rigidbody _rigidbody => GetComponent<Rigidbody>();
    private MassCenter _massCenter => GetComponent<MassCenter>();
    private Suspension _suspension => GetComponent<Suspension>();

    

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (!Physics.Raycast(this.transform.position, this.transform.up * -1, out hit, 1, _suspension.groundLayerMask))
            return;

        if(Input.GetKey(KeyCode.W))
        {
            Vector3 forwardMaxForce = maxSpeed * _massCenter.centerOfMass.forward;
            Vector3 forwardForce = Vector3.Lerp(Vector3.zero, forwardMaxForce, Time.fixedDeltaTime * acceleration);
            _rigidbody.AddForce(forwardMaxForce, ForceMode.Acceleration);
            
            if(Input.GetButton("Horizontal"))
            {
                float horizontal = Input.GetAxis("Horizontal");
                Vector3 maxTorque = _massCenter.centerOfMass.up * horizontal * steerAmount;
                Vector3 torque = Vector3.Lerp(Vector3.zero, maxTorque, Time.fixedDeltaTime * steering);
                _rigidbody.AddTorque(torque, ForceMode.Force);
            }
        }

        if(Input.GetButton("Jump"))
        {
            
        }
    }
}
