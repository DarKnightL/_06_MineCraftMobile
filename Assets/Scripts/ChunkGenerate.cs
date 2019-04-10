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

    Mesh mesh;


    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "Chunk";

        AddCubeFront();
        AddCubeBack();


        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangulars.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }


    void AddCubeFront()
    {


        triangulars.Add(0 + vertices.Count);
        triangulars.Add(3 + vertices.Count);
        triangulars.Add(2 + vertices.Count);

        triangulars.Add(2 + vertices.Count);
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(0 + vertices.Count);

        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(0, 1, 1));
        vertices.Add(new Vector3(0, 1, 0));
    }



    void AddCubeBack() {

        triangulars.Add(0 + vertices.Count);
        triangulars.Add(3 + vertices.Count);
        triangulars.Add(2 + vertices.Count);

        triangulars.Add(2 + vertices.Count);
        triangulars.Add(1 + vertices.Count);
        triangulars.Add(0 + vertices.Count);


        vertices.Add(new Vector3(-1, 0, 0));
        vertices.Add(new Vector3(-1, 0, 1));
        vertices.Add(new Vector3(-1, 1, 1));
        vertices.Add(new Vector3(-1, 1, 0));
    }
}
