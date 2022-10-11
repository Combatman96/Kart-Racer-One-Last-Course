using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    [SerializeField] Transform _suspensions;

    [SerializeField] private float _length;
    [SerializeField] private float _restDistance;
    [SerializeField] private float _strength;
    [SerializeField] private float _damper;
    [SerializeField] private LayerMask _groundLayerMask;

    private Rigidbody _carRigidbody => GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        foreach (Transform suspension in _suspensions)
        {
            Debug.DrawRay(suspension.position, this.transform.up * -1 * _length, Color.green);

            RaycastHit hit;
            bool isRayHit = Physics.Raycast(suspension.position, this.transform.up * -1, out hit, _length, _groundLayerMask);
            if(isRayHit)
            {
                Vector3 springDir = suspension.up;
                Vector3 springVel = _carRigidbody.GetPointVelocity(suspension.position);
                float offet = _length - hit.distance;
                float velocity = Vector3.Dot(springDir, springVel);
                float force = (offet * _strength) - (velocity * _damper);
                _carRigidbody.AddForceAtPosition(springDir * force, suspension.position);
            }
        }
    }
}
