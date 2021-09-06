using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;
    [SerializeField]
    Camera cam = null;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            posDelta = Input.mousePosition - prevPos;
            if (Vector3.Dot(transform.up, Vector3.up) > 0)
            {
                transform.Rotate(transform.up, -Vector3.Dot(posDelta, cam.transform.right), Space.World);
            }
            else
            {
                transform.Rotate(transform.up, Vector3.Dot(posDelta, cam.transform.right), Space.World);
            }

            transform.Rotate(cam.transform.right, Vector3.Dot(posDelta, cam.transform.up), Space.World);
        }

        prevPos = Input.mousePosition;
    }

    //void RotateObjectWithMouse()
    //{
    //    //Get mouse position
    //    Vector3 mousePos = Input.mousePosition;

    //    //Adjust mouse z position
    //    //mousePos.z = cam.transform.position.y - transform.position.y;

    //    //Get a world position for the mouse
    //    Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);

    //    //Get the angle to rotate and rotate
    //    float angle = -Mathf.Atan2(transform.position.z - mouseWorldPos.z, transform.position.x - mouseWorldPos.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed * Time.deltaTime);
    //}

}
