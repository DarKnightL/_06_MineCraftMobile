using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject chunkPrefab;
    [SerializeField]
    private int viewRange=30;
    [SerializeField]
    private int columnHeight = 8;


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
                SpawnChunks(xx, zz);
            }
        }
    }


    void SpawnChunks(int xx,int zz) {
        for (int y = 0; y < columnHeight; y += Chunk.height) //unlock the limitation of chunk generation in Y axis
        {
            int yr = y * Chunk.height-1;
            Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(xx), Mathf.FloorToInt(yr), Mathf.FloorToInt(zz));
            if (chunk == null)
            {
                Instantiate(chunkPrefab, new Vector3(xx, Mathf.FloorToInt(yr), zz), Quaternion.identity);
            }
        }
    }
}
