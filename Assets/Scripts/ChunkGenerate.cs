using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(BoxCollider))]

public class ChunkGenerate : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangulars = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

   

    Mesh mesh;

    public Block[,,] map;

    [SerializeField]
    int height = 10;
    [SerializeField]
    public static int width = 20;
    [SerializeField]
    float textureOffset = 1 / 16f;
    [SerializeField]
    float shrinkSize = 0.005f;

    private void Start()
    {

        CalculateMap();


    }


    void CalculateMap()
    {
        map = new Block[width, height, width];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    if (y==height-1&&Random.Range(0,5)==1)
                    {
                        map[x, y, z] = BlockList.GetBlock("grass");
                    }
                    else if(y<height-1)
                    {
                        map[x, y, z] = BlockList.GetBlock("dirt");
                    }
                    
                }
            }
        }

        mesh = new Mesh();
        mesh.name = "Chunk";

        CalculateMesh();
    }


    void CalculateMesh()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    if (map[x, y, z] != null)
                    {
                        if (isBlockTransparant(x + 1, y, z))
                        {
                            AddCubeFront(x, y, z, map[x, y, z]);
                        }

                        if (isBlockTransparant(x - 1, y, z))
                        {
                            AddCubeBack(x, y, z, map[x, y, z]);
                        }

                        if (isBlockTransparant(x, y, z + 1))
                        {
                            AddCubeRight(x, y, z, map[x, y, z]);
                        }

                        if (isBlockTransparant(x, y, z - 1))
                        {
                            AddCubeLeft(x, y, z, map[x, y, z]);
                        }

                        if (isBlockTransparant(x, y + 1, z))
                        {
                            AddCubeTop(x, y, z, map[x, y, z]);
                        }

                        if (isBlockTransparant(x, y - 1, z))
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

        uvs.Add(new Vector2(b.textureX * textureOffset, b.textureY * textureOffset)+new Vector2(shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX * textureOffset) + textureOffset, b.textureY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
        uvs.Add(new Vector2((b.textureX * textureOffset) + textureOffset, (b.textureY * textureOffset) + textureOffset)+new Vector2(-shrinkSize, -shrinkSize));
        uvs.Add(new Vector2(b.textureX * textureOffset, (b.textureY * textureOffset) + textureOffset)+new Vector2(shrinkSize,-shrinkSize));


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
        uvs.Add(new Vector2(b.textureX * textureOffset, (b.textureY * textureOffset) + textureOffset)+ new Vector2(shrinkSize, -shrinkSize));
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



    public bool isBlockTransparant(int x, int y, int z)
    {
        if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
        {
            return true;
        }
        if (map[x, y, z] == null)
        {
            return true;
        }
        return false;

    }


}
