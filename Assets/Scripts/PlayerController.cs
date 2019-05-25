using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject chunkPrefab;
    [SerializeField]
    private int viewRange = 30;
    [SerializeField]
    private int columnHeight = 8;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GameObject highLightBlock;


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


        BlockController();
    }


    void SpawnChunks(int xx, int zz)
    {
        for (int y = 0; y < columnHeight; y++) //unlock the limitation of chunk generation in Y axis
        {
            int yr = y * Chunk.height - 1;
            Chunk chunk = Chunk.GetChunk(Mathf.FloorToInt(xx), Mathf.FloorToInt(yr), Mathf.FloorToInt(zz));
            if (chunk == null)
            {
                Instantiate(chunkPrefab, new Vector3(xx, yr, zz), Quaternion.identity);
            }
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
        }else
        {
            highLightBlock.transform.position = new Vector3(-1000, -1000, -1000);
        }



    }
}
