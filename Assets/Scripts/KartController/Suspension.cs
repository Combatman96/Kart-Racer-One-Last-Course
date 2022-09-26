using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public List<Transform> suspensions;

    [SerializeField]
    private float _suspensionLength;
    [SerializeField]
    private float _suspensionStrength;

    [SerializeField]
    private LayerMask _groundLayerMask;

    private Rigidbody _rigidbody => GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        foreach (Transform suspension in suspensions)
        {
            Debug.DrawRay(suspension.position, this.transform.up * -1 * _suspensionLength, Color.green);

            RaycastHit hit;
            if(Physics.Raycast(suspension.position, this.transform.up * -1, out hit, _suspensionLength, _groundLayerMask))
            {
                float compresstionRatio = (_suspensionLength - Vector3.Distance(hit.point, suspension.position)) / _suspensionLength;
                Vector3 suspensionForce = _suspensionStrength * Vector3.up * compresstionRatio;
                _rigidbody.AddForceAtPosition(suspensionForce, suspension.position, ForceMode.Force);
            }
        }
    }
}
