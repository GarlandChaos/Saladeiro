using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform targetToFollow = null;
    Rigidbody targetToFollowRigidbody = null;
    [SerializeField]
    Transform mainCamera = null;
    [SerializeField]
    Camera defaultCameraPosition = null;
    Vector3 velocity = Vector3.zero;
    Vector3 distanceFromTarget = Vector3.zero;
    [SerializeField]
    float followTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        targetToFollowRigidbody = targetToFollow.GetComponent<Rigidbody>();
        distanceFromTarget = targetToFollow.position - transform.position;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //Vector3 target = targetToFollowRigidbody.position - distanceFromTarget;
    //    //mainCamera.position = Vector3.SmoothDamp(mainCamera.position, target, ref velocity, followTime);    
    //}

    //private void FixedUpdate()
    //{
    //    Vector3 target = targetToFollow.position - distanceFromTarget;
    //    mainCamera.position = Vector3.SmoothDamp(mainCamera.position, target, ref velocity, followTime);
    //    //mainCamera.position = Vector3.Lerp(mainCamera.position, target, Time.fixedDeltaTime);
    //}

    private void LateUpdate()
    {
        Vector3 target = targetToFollow.position - distanceFromTarget;
        mainCamera.position = Vector3.SmoothDamp(mainCamera.position, target, ref velocity, followTime);
    }
}
