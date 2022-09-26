using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public List<Transform> suspensions;

    public float suspensionLength;
    [SerializeField]
    private float _suspensionStrength;
    public LayerMask groundLayerMask;

    private Rigidbody _rigidbody => GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        foreach (Transform suspension in suspensions)
        {
            Debug.DrawRay(suspension.position, this.transform.up * -1 * suspensionLength, Color.green);

            RaycastHit hit;
            if(Physics.Raycast(suspension.position, this.transform.up * -1, out hit, suspensionLength, groundLayerMask))
            {
                float compresstionRatio = (suspensionLength - Vector3.Distance(hit.point, suspension.position)) / suspensionLength;
                Vector3 suspensionForce = _suspensionStrength * Vector3.up * compresstionRatio;
                _rigidbody.AddForceAtPosition(suspensionForce, suspension.position, ForceMode.Force);
            }
        }
    }
}
