using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3 id;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public void BuildInit(Vector3 id, Vector3[] perlinData)
    {
        this.id = id;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();

        mesh.vertices = MeshAPI.ResizePerlinVerticesDown(perlinData);
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(LowPolyTerrain2D.instance.chunk_size);
        mesh.RecalculateNormals();
        meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }

    private void OnDestroy()
    {
        MyDestroy();
    }
    private void MyDestroy()
    {
        Destroy(meshFilter.mesh);
        Destroy(meshFilter.sharedMesh);
        Destroy(GetComponent<MeshCollider>().sharedMesh);
        Destroy(GetComponent<MeshCollider>());
        Destroy(this);
    }

}
