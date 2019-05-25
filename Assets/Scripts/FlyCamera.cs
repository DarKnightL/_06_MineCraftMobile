using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{
    public float Speed;
    public float xSpeed = 200;

    public float ySpeed = 200;
    public float yMinLimit = -50;
    public float yMaxLimit = 50;

    public float x = 0.0f;
    public float y = 0.0f;
    float damping = 5.0f;


    // Update is called once per frame
    void Update()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        Vector3 cf = this.transform.forward;
        Vector3 cr = this.transform.right;
        this.transform.position += cf * Time.deltaTime * V * Speed;
        this.transform.position += cr * Time.deltaTime * H * Speed;

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
        }
        Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping * Speed);

    }
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
