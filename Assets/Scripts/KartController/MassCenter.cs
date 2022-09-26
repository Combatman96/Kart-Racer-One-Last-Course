using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCenter : MonoBehaviour
{
    public Transform centerOfMass;
    public bool awake;
    private Rigidbody _rigidbody => GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        _rigidbody.WakeUp();
        awake = !_rigidbody.IsSleeping();
    }
}
