using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    public Transform gameObjectAll;

    IEnumerator Open(){
        foreach (Transform go in gameObjectAll)
        {
            if (go.gameObject.activeSelf==false)
            {
                go.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
        }


       
    }



    void Start()
    {
        StartCoroutine(Open());
    }


    void Update()
    {
        
    }
}
