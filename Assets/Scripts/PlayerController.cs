using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject chunkPrefab;
    [SerializeField]
    private int viewRange = 60;
    [SerializeField]
    private int columnHeight = 8;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GameObject highLightBlock;

    private bool once = true;


    void Start()
    {

    }


    void Update()
    {
        if (once && Mathf.FloorToInt(Time.time) % 5 == 0)
        {
            for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += Chunk.width)
            {
                for (float y = transform.position.y - viewRange; y < transform.position.y + viewRange; y += Chunk.height)
                {
                    for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += Chunk.width)
                    {
                        int xx = Mathf.FloorToInt((x / Chunk.width) * Chunk.width);
                        int yy = Mathf.FloorToInt((y / Chunk.height) * Chunk.height);
                        int zz = Mathf.FloorToInt((z / Chunk.width) * Chunk.width);
                        Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(xx), Mathf.FloorToInt(yy), Mathf.FloorToInt(zz));
                        if (chunk == null)
                        {
                            Instantiate(chunkPrefab, new Vector3(xx, yy, zz), Quaternion.identity);
                        }

                    }
                }

            }
            once = false;
        }
        else
        {
            once = true;
        }
        BlockController();
        SpawnChunks();
    }

    //void SpawnChunks(int xx, int zz)
    //{
    //    for (int y = 0; y < columnHeight; y++) //unlock the limitation of chunk generation in Y axis
    //    {
    //        int yr = y * Chunk.height - 1;
    //        Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(xx), Mathf.FloorToInt(yr), Mathf.FloorToInt(zz));
    //        if (chunk == null)
    //        {
    //            Instantiate(chunkPrefab, new Vector3(xx, yr, zz), Quaternion.identity);
    //        }
    //    }
    //}




    void SpawnChunks()
    {
        if (Chunk.isWorking)
        {
            return;
        }
        float lastDistance = 999999;
        Chunk chunk = null;
        for (int i = 0; i < Chunk.chunks.Count; i++)
        {
            float distance = Vector3.Distance(this.transform.position, Chunk.chunks[i].transform.position);
            if (distance < lastDistance)
            {
                Chunk cc = Chunk.chunks[i].GetComponent<Chunk>();
                if (cc.isReady == false)
                {
                    lastDistance = distance;
                    chunk = cc;
                }
            }
        }
        if (chunk != null)
        {
            chunk.StartFunction();
        }
    }



    void BlockController()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.transform.position, transform.forward, out hitInfo, 18f, layerMask))
        {
            Vector3 blockPos = hitInfo.point - hitInfo.normal / 2;  //the normal of the face the ray hit
            highLightBlock.transform.position = new Vector3(Mathf.Floor(blockPos.x), Mathf.Floor(blockPos.y), Mathf.Floor(blockPos.z));
            if (Input.GetMouseButtonDown(0))
            {
                Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(hitInfo.point.x), Mathf.FloorToInt(hitInfo.point.y), Mathf.FloorToInt(hitInfo.point.z));
                chunk.SetBlock(blockPos, null);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(hitInfo.point.x), Mathf.FloorToInt(hitInfo.point.y), Mathf.FloorToInt(hitInfo.point.z));
                chunk.SetBlock(blockPos, BlockList.GetBlock("dirt"));
            }
        }
        else
        {
            highLightBlock.transform.position = new Vector3(-1000, -1000, -1000);
        }



    }
}
