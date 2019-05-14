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

    private Quaternion screenMovementSpace;
    private Vector3 screenMovementForward, screenMovementRight;

    public Transform mainCam;


    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void Update()
    {

        screenMovementSpace = Quaternion.Euler(0, mainCam.eulerAngles.y, 0);
        screenMovementForward = screenMovementSpace * Vector3.forward;
        screenMovementRight = screenMovementSpace * Vector3.right;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        this.transform.position += screenMovementForward * v * speed * Time.deltaTime;
        this.transform.position += screenMovementRight * h * speed * Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * speedX;
            y += Input.GetAxis("Mouse Y") * speedY * -1f;
            y = clampAngle(yMin, yMax, y);

        }

        //Quaternion rotationQ = Quaternion.Euler(y, x, 0f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotationQ, Time.deltaTime * speed);
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
