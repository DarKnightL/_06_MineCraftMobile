using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(BoxCollider))]

public class Chunk : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangulars = new List<int>();
    List<Vector2> uvs = new List<Vector2>();


    public static List<Transform> chunks = new List<Transform>();



    Mesh mesh;


    Chunk bottom;
    Chunk top;
    Chunk left;
    Chunk right;
    Chunk front;
    Chunk back;


    public Block[,,] map;

    [SerializeField]
    public static int height = 16;
    [SerializeField]
    public static int width = 16;
    [SerializeField]
    float textureOffset = 1 / 16f;
    [SerializeField]
    float shrinkSize = 0.005f;



    public static bool isWorking = false;
    public static bool blockWorking = false;


    public bool isReady = false;
    private GameObject gamePlayer;

    public static int seed;


    void Start()
    {
        chunks.Add(this.transform);
    }



    private void Update()
    {
        if (gamePlayer == null)
        {
            gamePlayer = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            if (Vector3.Distance(this.transform.position, gamePlayer.transform.position) > 100f)
            {
                chunks.Remove(this.transform);
                Destroy(this.gameObject);
            }
        }



        //if (isReady == false && isWorking == false)
        //{
        //    isReady = true;
        //    StartFunction();
        //}
    }


    public void StartFunction()
    {

        isWorking = true;
        mesh = new Mesh();
        mesh.name = "Chunk";
        map = new Block[width, height, width];
        StartCoroutine(CalculateMap());

    }


    public IEnumerator CalculateMap()
    {

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {

                for (int z = 0; z < width; z++)
                {
                    Block block = GetTheoreticalBlock(new Vector3(x, y, z) + transform.position);
                    if (block != null)
                    {
                        if (GetTheoreticalBlock(new Vector3(x, y + 1, z) + transform.position) == null)
                        {
                            map[x, y, z] = BlockList.GetBlock("grass");
                        }
                        else
                        {
                            map[x, y, z] = block;
                        }
                        //map[x, y, z] = GetTheoreticalBlock(new Vector3(x, y, z) + transform.position);
                    }




                    //if (y == height - 1 && Random.Range(0, 5) == 1)
                    //{
                    //    map[x, y, z] = BlockList.GetBlock("grass");
                    //}
                    //else if (y < height - 1)
                    //{
                    //    map[x, y, z] = BlockList.GetBlock("dirt");
                    //}
                }

            }
        }

        yield return null;

        StartCoroutine(ReCalculateMesh());
    }






    public IEnumerator ReCalculateMesh()
    {
        isReady = true;
        blockWorking = true;
        mesh = new Mesh();
        vertices.Clear();
        triangulars.Clear();
        uvs.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {


                    if (map[x, y, z] != null)
                    {
                        //if (y < 4)
                        //{
                        //    continue;
                        //}

                        if (PrimeBlockTransparant(x + 1, y, z))
                        {
                            AddCubeFront(x, y, z, map[x, y, z]);
                        }

                        if (PrimeBlockTransparant(x - 1, y, z))
                        {
                            AddCubeBack(x, y, z, map[x, y, z]);
                        }

                        if (PrimeBlockTransparant(x, y, z + 1))
                        {
                            AddCubeRight(x, y, z, map[x, y, z]);
                        }

                        if (PrimeBlockTransparant(x, y, z - 1))
                        {
                            AddCubeLeft(x, y, z, map[x, y, z]);
                        }

                        if (PrimeBlockTransparant(x, y + 1, z))
                        {
                            AddCubeTop(x, y, z, map[x, y, z]);
                        }

                        if (PrimeBlockTransparant(x, y - 1, z))
                        {

                            AddCubeBottom(x, y, z, map[x, y, z]);
                        }
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangulars.ToArray();
        mesh.uv = uvs.ToArray();


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        this.gameObject.AddComponent<BoxCollider>();



        yield return null;
        isWorking = false;

        blockWorking = false;
        yield return null;
    }




    public void SetBlock(Vector3 worldPos, Block block)
    {
        Vector3 localPos = worldPos - transform.position;
        if (localPos.x > worldPos.x)
        {
            return;
        }
        if (Mathf.FloorToInt(localPos.x) >= width || Mathf.FloorToInt(localPos.y) >= height || Mathf.FloorToInt(localPos.z) >= width || Mathf.FloorToInt(localPos.x) < 0 || Mathf.FloorToInt(localPos.y) < 0 || Mathf.FloorToInt(localPos.z) < 0)
        {

        }
        else
        {
            map[Mathf.FloorToInt(localPos.x), Mathf.FloorToInt(localPos.y), Mathf.FloorToInt(localPos.z)] = block;
        }
        //isWorking = true;
        StartCoroutine(ReCalculateMesh());

        if (block != null)
        {
            return;
        }


        if (Mathf.FloorToInt(localPos.x) >= width - 1)
        {
            if (right == null)
            {
                right = GetChunk(Mathf.FloorToInt(localPos.x + 1), Mathf.FloorToInt(localPos.y), Mathf.FloorToInt(localPos.z));
            }
            StartCoroutine(right.ReCalculateMesh());
        }
        if (Mathf.FloorToInt(localPos.x) <= 1)
        {
            if (left == null)
            {
                left = GetChunk(Mathf.FloorToInt(localPos.x - 1), Mathf.FloorToInt(localPos.y), Mathf.FloorToInt(localPos.z));
            }
            StartCoroutine(left.ReCalculateMesh());
        }
        if (Mathf.FloorToInt(localPos.z) >= width - 1)
        {
            if (front == null)
            {
                front = GetChunk(Mathf.FloorToInt(localPos.x), Mathf.FloorToInt(localPos.y), Mathf.FloorToInt(localPos.z + 1));
            }
            StartCoroutine(front.ReCalculateMesh());
        }
        if (Mathf.FloorToInt(localPos.z) <= 1)
        {
            if (back == null)
            {
                back = GetChunk(Mathf.FloorToInt(localPos.x), Mathf.FloorToInt(localPos.y), Mathf.FloorToInt(localPos.z - 1));
            }
            StartCoroutine(back.ReCalculateMesh());
        }
        if (Mathf.FloorToInt(localPos.y) >= height - 1)
        {
            if (top == null)
            {
                top = GetChunk(Mathf.FloorToInt(localPos.x), Mathf.FloorToInt(localPos.y + 1), Mathf.FloorToInt(localPos.z));
            }
            StartCoroutine(top.ReCalculateMesh());
        }
        if (Mathf.FloorToInt(localPos.y) <= 1)
        {
            if (bottom == null)
            {
                bottom = GetChunk(Mathf.FloorToInt(localPos.x), Mathf.FloorToInt(localPos.y - 1), Mathf.FloorToInt(localPos.z));
            }
            StartCoroutine(bottom.ReCalculateMesh());
        }
    }




    void AddCubeFront(int x, int y, int z, Block b)
    {


        triangulars.Add(0 + vertices.Count);
        triangulars.Add(3 + vertices.Count);
        triangulars.Add(2 + vertices.Count);

        triangulars.Add(2 + vertices.Count);
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(0 + vertices.Count);

        vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
        vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
        vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
        vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));

        uvs.Add(new Vector2(b.textureX * textureOffset, b.textureY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX * textureOffset) + textureOffset, b.textureY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX * textureOffset) + textureOffset, (b.textureY * textureOffset) + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX * textureOffset, (b.textureY * textureOffset) + textureOffset) + new Vector2(shrinkSize, -shrinkSize));


    }


    void AddCubeBack(int x, int y, int z, Block b)
    {

        triangulars.Add(1 + vertices.Count);
        triangulars.Add(2 + vertices.Count);
        triangulars.Add(3 + vertices.Count);

        triangulars.Add(3 + vertices.Count);
        triangulars.Add(0 + vertices.Count);
        triangulars.Add(1 + vertices.Count);


        vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
        vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));

        uvs.Add(new Vector2(b.textureX * textureOffset, b.textureY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX * textureOffset) + textureOffset, b.textureY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX * textureOffset) + textureOffset, (b.textureY * textureOffset) + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX * textureOffset, (b.textureY * textureOffset) + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
    }


    void AddCubeRight(int x, int y, int z, Block b)
    {

        triangulars.Add(0 + vertices.Count);
        triangulars.Add(3 + vertices.Count);
        triangulars.Add(2 + vertices.Count);

        triangulars.Add(2 + vertices.Count);
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(0 + vertices.Count);


        vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
        vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));

        uvs.Add(new Vector2(b.textureX_LR * textureOffset, b.textureY_LR * textureOffset) + new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_LR * textureOffset) + textureOffset, b.textureY_LR * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_LR * textureOffset) + textureOffset, (b.textureY_LR * textureOffset) + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX_LR * textureOffset, (b.textureY_LR * textureOffset) + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
    }


    void AddCubeLeft(int x, int y, int z, Block b)
    {
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(2 + vertices.Count);
        triangulars.Add(3 + vertices.Count);

        triangulars.Add(3 + vertices.Count);
        triangulars.Add(0 + vertices.Count);
        triangulars.Add(1 + vertices.Count);


        vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
        vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
        vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));
        vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));

        uvs.Add(new Vector2(b.textureX_LR * textureOffset, b.textureY_LR * textureOffset) + new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_LR * textureOffset) + textureOffset, b.textureY_LR * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_LR * textureOffset) + textureOffset, (b.textureY_LR * textureOffset) + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX_LR * textureOffset, (b.textureY_LR * textureOffset) + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
    }


    void AddCubeTop(int x, int y, int z, Block b)
    {
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(0 + vertices.Count);
        triangulars.Add(3 + vertices.Count);

        triangulars.Add(3 + vertices.Count);
        triangulars.Add(2 + vertices.Count);
        triangulars.Add(1 + vertices.Count);


        vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));
        vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));

        uvs.Add(new Vector2(b.textureX_Top * textureOffset, b.textureY_Top * textureOffset) + new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_Top * textureOffset) + textureOffset, b.textureY_Top * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_Top * textureOffset) + textureOffset, (b.textureY_Top * textureOffset) + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX_Top * textureOffset, (b.textureY_Top * textureOffset) + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
    }


    void AddCubeBottom(int x, int y, int z, Block b)
    {
        triangulars.Add(2 + vertices.Count);
        triangulars.Add(3 + vertices.Count);
        triangulars.Add(0 + vertices.Count);

        triangulars.Add(0 + vertices.Count);
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(2 + vertices.Count);


        vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
        vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
        vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));

        uvs.Add(new Vector2(b.textureX_Bottom * textureOffset, b.textureY_Bottom * textureOffset) + new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_Bottom * textureOffset) + textureOffset, b.textureY_Bottom * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX_Bottom * textureOffset) + textureOffset, (b.textureY_Bottom * textureOffset) + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX_Bottom * textureOffset, (b.textureY_Bottom * textureOffset) + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
    }



    public bool PrimeBlockTransparant(int x, int y, int z)
    {
        if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
        {
            if (GetTheoreticalBlock(new Vector3(x, y, z) + transform.position) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (map[x, y, z] == null)
        {
            return true;
        }

        return false;
    }





    public bool isBlockTransparant(int x, int y, int z)
    {
        if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
        {
            if (GetTheoreticalBlock(new Vector3(x, y, z) + transform.position) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (map[x, y, z] == null)
        {
            return true;
        }

        return false;
    }



    public static Chunk GetChunk(int x, int y, int z)
    {

        for (int i = 0; i < chunks.Count; i++)
        {
            Vector3 pos = new Vector3(x, y, z);
            Vector3 chunkPos = chunks[i].transform.position;
            if (chunkPos.Equals(pos))
            {
                return chunks[i].GetComponent<Chunk>();
            }

            if (pos.x < chunkPos.x || pos.y < chunkPos.y || pos.z < chunkPos.z || pos.x > chunkPos.x + Chunk.width || pos.y > chunkPos.y + Chunk.height || pos.z > chunkPos.z + Chunk.width)
            {
                continue;
            }
            return chunks[i].GetComponent<Chunk>();

        }

        return null;
    }


    public Block GetTheoreticalBlock(Vector3 pos)
    {
        Random.InitState(seed);
        Vector3 offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        float noiseX = Mathf.Abs((float)(pos.x + offset.x) / 20);
        float noiseY = Mathf.Abs((float)(pos.y + offset.y) / 20);
        float noiseZ = Mathf.Abs((float)(pos.z + offset.z) / 20);

        float noiseValue = SimplexNoise.Noise.Generate(noiseX, noiseY, noiseZ);
        noiseValue += (20 - (float)pos.y) / 18f;
        noiseValue /= ((float)pos.y) / 19f;
        if (noiseValue > 0.2f)
        {
            return BlockList.GetBlock("dirt");
        }
        return null;
    }




}
