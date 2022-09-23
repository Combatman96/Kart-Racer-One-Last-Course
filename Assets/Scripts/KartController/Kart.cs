using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    public Transform kartMeshTransform;
    public Transform colliderTransform;

    private void FixedUpdate()
    {
        kartMeshTransform.position = colliderTransform.position;
    }
}
