using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidbody = null;

    void ClearRigidbodyValues()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ClearRigidbodyValues();
    }

    private void OnCollisionExit(Collision collision)
    {
        ClearRigidbodyValues();
    }
}
