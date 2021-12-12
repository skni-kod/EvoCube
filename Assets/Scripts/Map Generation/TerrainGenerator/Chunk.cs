using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3 id;
    public LowPolyTerrain terrainReference;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [SerializeField] public int size = 128;

    public void Init(Vector3 id)
    {
        this.id = id;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();

        mesh.vertices = MeshAPI.CreateVerticesFlat(size + 1, 1, PerlinAPI.GPUPerlin2D(size + 1, id*size));
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(size);
        mesh.RecalculateNormals();
        meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }

    public void BuildInit(Vector3[] vertices, int[] triangles)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }


}
