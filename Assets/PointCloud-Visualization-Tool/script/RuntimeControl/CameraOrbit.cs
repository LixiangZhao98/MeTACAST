using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Required")]
    public Transform target;

    [Header("Config")]
    public float targetdistance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float ScrollSensativity = 4f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;


    //For smoothness
    float targetx = 0.0f;
    float targety = 0.0f;
    float distance = 5f;



    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(0))
            {
                targetx += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f * (5 / (distance + 2));
                targety -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }


            targety = ClampAngle(targety, yMinLimit, yMaxLimit);

            x = Mathf.LerpAngle(x, targetx, 0.1f);
            y = Mathf.LerpAngle(y, targety, 1f);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            targetdistance = Mathf.Clamp(targetdistance - (Input.GetAxis("Mouse ScrollWheel") * ScrollSensativity), distanceMin, distanceMax);
            distance = Mathf.Lerp(distance, targetdistance, 0.1f); //Smooth

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                targetdistance -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;

            transform.position = position;
        }


        //Change fov 
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            GetComponent<Camera>().fieldOfView += 0.3f;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            GetComponent<Camera>().fieldOfView -= 0.3f;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}