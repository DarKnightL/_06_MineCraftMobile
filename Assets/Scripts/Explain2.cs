using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explain2 : MonoBehaviour
{
  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Camera.main.transform.position,transform.forward,out hitInfo,Mathf.Infinity))
            {
                Debug.DrawLine(Camera.main.transform.position, hitInfo.transform.position);
            }
        }
    }
}
