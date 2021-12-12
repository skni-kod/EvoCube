using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3 id;
    public LowPolyTerrain terrainReference;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public void Init(Vector3 id)
    {
        this.id = id;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();

        mesh.vertices = MeshAPI.CreateVerticesFlat(LowPolyTerrain.instance.chunk_size + 1, 1, PerlinAPI.GPUPerlin2D(LowPolyTerrain.instance.chunk_size + 1, id* LowPolyTerrain.instance.chunk_size));
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(LowPolyTerrain.instance.chunk_size);
        mesh.RecalculateNormals();
        meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }

    public void BuildInit(Vector3 id, Vector3[] perlinData)
    {
        this.id = id;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();

        mesh.vertices = MeshAPI.ResizePerlinVerticesDown(perlinData);
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(LowPolyTerrain.instance.chunk_size);
        mesh.RecalculateNormals();
        meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
        if (LowPolyTerrain.instance.spawn_later.Contains(id))
        {
            LowPolyTerrain.instance.spawn_later.Remove(id);
        }
    }

    public void MyDestroy()
    {
        Destroy(meshFilter.mesh);
        Destroy(meshFilter.sharedMesh);
        Destroy(GetComponent<MeshCollider>().sharedMesh);
        Destroy(GetComponent<MeshCollider>());
        Destroy(this);
    }

}
