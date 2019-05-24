using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTest : MonoBehaviour
{
    public int width = 64;
    public int height = 64;
    public int heightScale = 20;
    public float detailScale = 25f;

    public GameObject Block;


    void Start()
    {
        for (int z = 0; z < width; z++)
        {
            for (int x = 0; x < height; x++)
            {
                int y = (int)(Mathf.PerlinNoise(x / detailScale, z / detailScale)* heightScale);
                //Debug.Log(y);
                Vector3 pos = new Vector3(x, y, z);
                Instantiate(Block,pos, Quaternion.identity);
            }
        }
    }


    void Update()
    {


    }
}
