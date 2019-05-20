using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject chunkPrefab;
    [SerializeField]
    private int viewRange=30;


    void Start()
    {

    }


    void Update()
    {
        for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += Chunk.width)
        {
            for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += Chunk.width)
            {
                int xx = Mathf.FloorToInt((x / Chunk.width) * Chunk.width);
                int zz = Mathf.FloorToInt((z / Chunk.width) * Chunk.width);
                Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(xx), 0, Mathf.FloorToInt(zz));
                if (chunk == null)
                {
                    Instantiate(chunkPrefab, new Vector3(xx, 0, zz), Quaternion.identity);
                }
            }
        }
    }
}
