using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public static List<Chunk> chunks = new List<Chunk>();


    void Start()
    {
        chunks.Add(this);
    }




    void Update()
    {

    }

    public static Chunk GetChunk(int x, int y, int z)
    {

        for (int i = 0; i < chunks.Count; i++)
        {
            Vector3 pos = new Vector3(x, y, z);
            Vector3 chunkPos = chunks[i].transform.position;
            if (chunkPos.Equals(pos))
            {
                return chunks[i];
            }

            if (pos.x < chunkPos.x || pos.y < chunkPos.y || pos.z < chunkPos.z || pos.x > chunkPos.x + ChunkGenerate.width || pos.y > chunkPos.y + ChunkGenerate.width || pos.z > chunkPos.z + ChunkGenerate.width)
            {
                continue;
            }
            return chunks[i];

        }
        return null;
    }
}
