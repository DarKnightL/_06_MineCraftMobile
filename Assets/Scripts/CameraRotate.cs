using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private float x = 0f;
    private float y = 0f;

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float speedX = 50f;
    [SerializeField]
    private float speedY = 50f;
    [SerializeField]
    private float yMin = -50f;
    [SerializeField]
    private float yMax = 50f;


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * speedX;
            y += Input.GetAxis("Mouse Y") * speedY * -1f;
            y = clampAngle(yMin, yMax, y);

        }

        Quaternion rotationQ = Quaternion.Euler(y, x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationQ, Time.deltaTime * speed);
    }


    private static float clampAngle(float min, float max, float angle)
    {

        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);

    }


}
