using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject chunkPrefab;
    [SerializeField]
    private int viewRange;


    void Start()
    {
        
    }


    void Update()
    {
        for (float x = transform.position.x-viewRange; x < transform.position.x+viewRange; x+=ChunkGenerate.width)
        {
            for (float z = transform.position.z-viewRange; z < transform.position.z+viewRange; z+=ChunkGenerate.width)
            {
                int xx = Mathf.FloorToInt(x / ChunkGenerate.width) * ChunkGenerate.width;
                int zz = Mathf.FloorToInt(z / ChunkGenerate.width) * ChunkGenerate.width;
                Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(xx),0, Mathf.FloorToInt(zz));
                if (chunk == null)
                {
                    Instantiate(chunkPrefab, new Vector3(Mathf.FloorToInt(xx), 0, Mathf.FloorToInt(zz)), Quaternion.identity);
                }
            }
        }
    }
}
